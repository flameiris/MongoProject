using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iris.FrameCore.RabbitMQ
{
    public class RabbitMqClient : IDisposable
    {
        private readonly ILogger _logger;
        private readonly IConnection _conn;
        private static readonly ConcurrentDictionary<string, IModel> ModelDic =
            new ConcurrentDictionary<string, IModel>();
        #region 消息无法消费时使用的专用队列
        //static int tryTimes = 3;
        static readonly string ExchangeName = "vmeng_exc";
        static readonly string QueueName = "vmeng_UnConsumer";
        static readonly string RouteKey = "bind_UnConsumer";
        static readonly string ExchangeType = "direct";

        #endregion

        public RabbitMqClient(
            ILogger<RabbitMqClient> logger,
            IOptions<RabbitMqOptions> settings)
        {
            _logger = logger;
            var factory = new ConnectionFactory
            {
                VirtualHost = "my_vhost",
                HostName = settings.Value.HostName,
                UserName = settings.Value.UserName,
                Password = settings.Value.PassWord,
                AutomaticRecoveryEnabled = settings.Value.AutomaticRecoveryEnabled,
                //重连后恢复当前的工作进程，比如channel、queue、发布的消息进度等 
                TopologyRecoveryEnabled = true,
                //随机化连接间隔，因此多个连接不会尝试同时重新连接。(10s-30s)
                NetworkRecoveryInterval = TimeSpan.FromSeconds(new Random().Next(10, 30)),
                RequestedHeartbeat = 15
            };
            _conn = factory.CreateConnection();
        }

        /// <summary>
        /// 获取MQ实体的Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private RabbitMQAttribute GetRabbitMqAttribute<T>()
        {
            var attrs = typeof(T).GetCustomAttributes(false);
            if (attrs == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<RabbitMQAttribute>(JsonConvert.SerializeObject(attrs[0]));
        }

        /// <summary>
        /// 获取信道（channel），使用信道创建并绑定交换机、队列
        /// </summary>
        /// <param name="exchange">交换机名称</param>
        /// <param name="queue">队列名称</param>
        /// <param name="routingKey"></param>
        /// <param name="exchangeType">匹配规则</param>
        /// <param name="durable">是否持久化</param>
        /// <returns></returns>
        private IModel GetModel(string exchange, string queue, string routingKey, string exchangeType, bool durable = true,
            bool autoDelete = false, IDictionary<string, object> exchangeArgs = null, IDictionary<string, object> queueArgs = null)
        {
            return ModelDic.GetOrAdd(queue, key =>
            {
                //用连接创建信道(channel)
                var model = _conn.CreateModel();
                //创建交换机
                ExchangeDeclare(model, exchange, exchangeType, durable, autoDelete, exchangeArgs);
                //创建队列
                QueueDeclare(model, queue, durable, autoDelete, queueArgs);
                model.QueueBind(queue, exchange, routingKey);
                ModelDic[queue] = model;
                return model;
            });
        }

        /// <summary>
        /// 创建交换机
        /// </summary>
        /// <param name="iModel"></param>
        /// <param name="exchange"></param>
        /// <param name="type"></param>
        /// <param name="durable"></param>
        /// <param name="autoDelete"></param>
        /// <param name="arguments"></param>
        private static void ExchangeDeclare(IModel iModel, string exchange, string type, bool durable = true,
            bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            exchange = string.IsNullOrWhiteSpace(exchange) ? "" : exchange.Trim();
            iModel.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
        }
        /// <summary>
        /// 创建队列
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queue"></param>
        /// <param name="durable"></param>
        /// <param name="autoDelete"></param>
        /// <param name="arguments"></param>
        /// <param name="exclusive"></param>
        private static void QueueDeclare(IModel channel, string queue, bool durable = true, bool autoDelete = false, IDictionary<string, object> arguments = null, bool exclusive = false)
        {
            queue = string.IsNullOrWhiteSpace(queue) ? "UndefinedQueueName" : queue.Trim();
            channel.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }
        /// <summary>
        /// 发送消息（同步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObj"></param>
        public void SendMessage<T>(ILogger _logger, T messageObj) where T : class
        {
            var queueInfo = GetRabbitMqAttribute<T>();
            if (queueInfo == null)
                throw new ArgumentException("消息上不具有任何特性");
            var channel = GetModel(queueInfo.ExchangeName, queueInfo.QueueName, queueInfo.RouteKey, queueInfo.ExchangeType, queueInfo.Durable);
            var properties = channel.CreateBasicProperties();

            properties.DeliveryMode = 2;
            //RabbitMQ的Ask机制
            channel.ConfirmSelect();
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageObj));
            channel.BasicPublish(queueInfo.ExchangeName, queueInfo.RouteKey, properties, body);
            bool isOk = channel.WaitForConfirms();
            if (!isOk)
            {
                _logger.LogError($"发布消息至对列确认失败。对列内容：{JsonConvert.SerializeObject(messageObj)}");
                throw new Exception();
            }
        }
        /// <summary>
        /// 发送消息（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObj"></param>
        public void SendMessageAsync<T>(ILogger _logger, T messageObj) where T : class
        {
            Task.Run(() =>
            {
                var queueInfo = GetRabbitMqAttribute<T>();
                if (queueInfo == null)
                    throw new ArgumentException("消息上不具有任何特性");
                var channel = GetModel(queueInfo.ExchangeName, queueInfo.QueueName, queueInfo.RouteKey, queueInfo.ExchangeType, queueInfo.Durable);
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                channel.ConfirmSelect();
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageObj));
                channel.BasicPublish(queueInfo.ExchangeName, queueInfo.RouteKey, properties, body);
                bool isOk = channel.WaitForConfirms();
                if (!isOk)
                {
                    _logger.LogError($"发布消息至对列确认失败。对列内容：{JsonConvert.SerializeObject(messageObj)}");
                    throw new Exception($"发布消息至对列确认失败。对列内容：{JsonConvert.SerializeObject(messageObj)}");
                }
            });
        }

        /// <summary>
        /// 异步发送消息+重试机制。 发送成功返回true; 发送失败返回false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObj"></param>
        /// <returns></returns>
        public async Task<bool> SendMessageWithReTryAsync<T>(T messageObj) where T : class
        {
            return await Task.Run(() =>
            {
                var sendCount = 3;
                bool flag = false;
                do
                {
                    try
                    {
                        var queueInfo = GetRabbitMqAttribute<T>();
                        if (queueInfo == null)
                            throw new ArgumentException("消息上不具有任何特性");
                        var channel = GetModel(queueInfo.ExchangeName, queueInfo.QueueName, queueInfo.RouteKey, queueInfo.ExchangeType, queueInfo.Durable);
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2;
                        channel.ConfirmSelect();
                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageObj));
                        channel.BasicPublish(queueInfo.ExchangeName, queueInfo.RouteKey, properties, body);
                        bool isOk = channel.WaitForConfirms();
                        if (!isOk)
                        {
                            throw new Exception($"发布消息至对列确认失败。对列内容：{JsonConvert.SerializeObject(messageObj)}");
                        }
                        flag = true;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"发布消息至对列确认失败。对列内容：{JsonConvert.SerializeObject(messageObj)}", e);
                        sendCount--;
                        Thread.Sleep(100);
                    }
                } while (sendCount > 0 && !flag);


                return flag;
            });
        }

        public void SendMessage(byte[] body)
        {
            var channel = GetModel(ExchangeName, QueueName, RouteKey, ExchangeType, true);
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2;
            channel.ConfirmSelect();
            channel.BasicPublish(ExchangeName, RouteKey, properties, body);
            bool isOk = channel.WaitForConfirms();
            if (!isOk)
            {
                throw new Exception($"发布消息至对列确认失败。{Encoding.Default.GetString(body)}");
            }
        }
        /// <summary>
        /// 发送延迟消息 通过插件rabbitmq-delayed-message-exchange实现
        /// http://blog.csdn.net/u014308482/article/details/53036770 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObj"></param>
        /// <param name="publishTime">消息推送时间，即服务接收到消息的时间</param>

        public void SendDelayMessageByPlugin<T>(T messageObj, DateTime publishTime) where T : class
        {
            long delay = (long)(publishTime - DateTime.Now).TotalMilliseconds;
            SendDelayMessageByPlugin<T>(messageObj, delay);
        }

        /// <summary>
        /// 发送延迟消息 通过插件rabbitmq-delayed-message-exchange实现
        /// http://blog.csdn.net/u014308482/article/details/53036770 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObj"></param>
        /// <param name="publishTime">消息推送时间，即服务接收到消息的时间</param>
        /// <param name="delay">延时的毫秒数</param>
        public void SendDelayMessageByPlugin<T>(T messageObj, long delay) where T : class
        {
            var queueInfo = GetRabbitMqAttribute<T>();
            if (queueInfo == null)
                throw new ArgumentException("消息上不具有任何特性");

            //延迟队列参数，必须
            IDictionary<string, object> args = new Dictionary<string, object>
            {
                {"x-delayed-type", "direct"}
            };
            var channel = GetModel(queueInfo.DelayExchangeName, queueInfo.DelayQueueName, queueInfo.DelayRouteKey, queueInfo.DelayExchangeType, queueInfo.Durable, false, args);
            var properties = channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object>
            {
                {"x-delay", delay}
            };
            properties.DeliveryMode = 2;
            channel.ConfirmSelect();
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageObj));
            channel.BasicPublish(queueInfo.DelayExchangeName, queueInfo.DelayRouteKey, properties, body);
            bool isOk = channel.WaitForConfirms();
            if (!isOk)
            {
                throw new Exception($"发布消息至对列确认失败。{JsonConvert.SerializeObject(messageObj)}");
            }
        }

        /// <summary>
        /// 发送延迟消息 通过死信实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObj"></param>
        /// <param name="publishTime">消息推送时间，即服务接收到消息的时间</param>

        public void SendDelayMessageByDeadLetter<T>(T messageObj, DateTime publishTime) where T : class
        {
            long delay = (long)(publishTime - DateTime.Now).TotalMilliseconds;
            SendDelayMessageByDeadLetter<T>(messageObj, delay);
        }

        /// <summary>
        /// 发送延迟消息 通过死信实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObj"></param>
        /// <param name="delay">延时的毫秒数</param>

        public void SendDelayMessageByDeadLetter<T>(T messageObj, long delay) where T : class
        {
            var queueInfo = GetRabbitMqAttribute<T>();
            if (queueInfo == null)
                throw new ArgumentException("消息上不具有任何特性");

            //延迟队列参数，必须
            IDictionary<string, object> queueArgs = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", queueInfo.DeadExchangeName},
                {"x-dead-letter-routing-key", queueInfo.DeadRouteKey}
            };

            var channel = ModelDic.GetOrAdd(queueInfo.QueueName, key =>
            {
                //声明原交换机，队列，并绑定
                var model = _conn.CreateModel();
                ExchangeDeclare(model, queueInfo.ExchangeName, queueInfo.ExchangeType, queueInfo.Durable, false, null);
                QueueDeclare(model, queueInfo.QueueName, queueInfo.Durable, false, queueArgs);
                model.QueueBind(queueInfo.QueueName, queueInfo.ExchangeName, queueInfo.RouteKey, null);

                //声明死信交换机，死信队列，并绑定
                ExchangeDeclare(model, queueInfo.DeadExchangeName, queueInfo.DeadExchangeType, queueInfo.Durable, false, null);
                QueueDeclare(model, queueInfo.DeadQueueName, queueInfo.Durable, false, null);
                model.QueueBind(queueInfo.DeadQueueName, queueInfo.DeadExchangeName, queueInfo.DeadRouteKey, null);

                ModelDic[queueInfo.QueueName] = model;
                return model;
            });

            var properties = channel.CreateBasicProperties();
            channel.ConfirmSelect();
            properties.Expiration = delay.ToString();//设置消息过期时间
            properties.DeliveryMode = 2;
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageObj));
            //发送消息到原队列
            channel.BasicPublish(queueInfo.ExchangeName, queueInfo.RouteKey, properties, body);
            var isOk = channel.WaitForConfirms();
            if (!isOk)
            {
                throw new Exception($"发布消息至对列确认失败。{JsonConvert.SerializeObject(messageObj)}");
            }
        }


        public void ReceiveMessage<T>(Func<T, CancellationToken, Task<bool>> handler)
        {

            var queueInfo = GetRabbitMqAttribute<T>();
            if (queueInfo == null)
                throw new ArgumentException("消息上不具有任何特性");
            if (handler == null)
            {
                throw new NullReferenceException("处理事件为null");
            }
            var channel = GetModel(queueInfo.ExchangeName, queueInfo.QueueName, queueInfo.RouteKey, queueInfo.ExchangeType, queueInfo.Durable);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queueInfo.QueueName, false, consumer);
            consumer.Received += async (model, ea) =>
            {
                await _doAsync(channel, ea, handler);

            };
        }
        private async Task _doAsync<T>(IModel channel, BasicDeliverEventArgs ea, Func<T, CancellationToken, Task<bool>> normalHandler)
        {
            var body = ea.Body;
            try
            {
                var msgBody = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                var isConsumeSuccess = await normalHandler(msgBody, default(CancellationToken));
                //此处如果处理失败 可以不确认消费 TODO
                //if (isConsumeSuccess)
                //{
                //    channel.BasicAck(ea.DeliveryTag, false);
                //}
                //else
                //{
                //决绝消费（重回对列）
                //    channel.BasicReject(ea.DeliveryTag, true);
                //}
                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                channel.BasicAck(ea.DeliveryTag, false);
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// 接收延迟消息，通过 通过插件rabbitmq-delayed-message-exchange实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void ReceiveDelayMessageByPlugin<T>(Func<T, CancellationToken, Task<bool>> handler)
        {
            var queueInfo = GetRabbitMqAttribute<T>();

            if (queueInfo == null)
                throw new ArgumentException("消息上不具有任何特性");
            if (handler == null)
            {
                throw new NullReferenceException("处理事件为null");
            }
            //延迟队列参数，必须

            IDictionary<string, object> args = new Dictionary<string, object>
            {
                {"x-delayed-type", "direct"}
            };
            var channel = GetModel(queueInfo.DelayExchangeName, queueInfo.DelayQueueName, queueInfo.DelayRouteKey, queueInfo.DelayExchangeType, queueInfo.Durable, false, args);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var msgBody = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(ea.Body));
                await handler(msgBody, default(CancellationToken));
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queueInfo.DelayQueueName, false, consumer);
        }


        /// <summary>
        /// 接收延迟消息 通过死信方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void ReceiveDelayMessageByDeadLetter<T>(Func<T, CancellationToken, Task<bool>> handler)
        {
            var queueInfo = GetRabbitMqAttribute<T>();

            if (queueInfo == null)
                throw new ArgumentException("消息上不具有任何特性");
            if (handler == null)
            {
                throw new NullReferenceException("处理事件为null");
            }
            var channel = GetModel(queueInfo.DeadExchangeName, queueInfo.DeadQueueName, queueInfo.DeadRouteKey, queueInfo.DeadExchangeType, queueInfo.Durable);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {

                    var msgBody = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(ea.Body));
                    var isOk = await handler(msgBody, default(CancellationToken));
                    if (isOk)
                    {

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
                catch (Exception)
                {

                    channel.BasicAck(ea.DeliveryTag, false);
                }

            };
            channel.BasicConsume(queueInfo.DeadQueueName, false, consumer);
        }


        public void Dispose()
        {
            foreach (var item in ModelDic)
            {
                item.Value.Close();
                item.Value.Dispose();
            }
            _conn.Dispose();
        }

        public void CloseConnection()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}

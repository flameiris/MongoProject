using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Kafka.Logs
{
    /// <summary>
    /// 生产者
    /// </summary>
    public interface IKafkaProducer : IDisposable
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="operateType"></param>
        /// <returns></returns>
        Task<bool> Produce<T>(string topic, string key, T data, int operateType) where T : class;
    }

    /// <summary>
    /// 消费者
    /// </summary>
    public interface IKafkaConsumer : IDisposable
    {
        /// <summary>
        /// 消费数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> Consume<T>() where T : class;
    }

    public class KafkaManager : IKafkaProducer, IKafkaConsumer
    {
        private readonly IProducer<string, string> _producer;
        private readonly IConsumer<string, string> _consumer;

        private bool disposeHasBeenCalled = false;
        private readonly object disposeHasBeenCalledLockObj = new object();

        /// <summary>
        /// 构造函数，初始化配置
        /// </summary>
        /// <param name="config">配置参数</param>
        /// <param name="topic">主题名称</param>
        public KafkaManager(ProducerConfig confProducer, ConsumerConfig confConsumer)
        {
            _producer = new ProducerBuilder<string, string>(confProducer).Build();
            _consumer = new ConsumerBuilder<string, string>(confConsumer).Build();
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T">数据实体</typeparam>
        /// <param name="key">数据key,partition分区会根据key</param>
        /// <param name="data">数据</param>
        /// <param name="operateType">操作类型[增、删、改等不同类型]</param>
        /// <returns></returns>
        public async Task<bool> Produce<T>(string topic, string key, T data, int operateType) where T : class
        {
            var obj = JsonConvert.SerializeObject(data);

            try
            {
                var result = await _producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = key,
                    Value = obj
                });

#if DEBUG
                Console.WriteLine($"Topic: {result.Topic} Partition: {result.Partition} Offset: {result.Offset}");
#endif

                _producer.Flush(TimeSpan.FromSeconds(10));
                return true;

            }
            catch (ProduceException<string, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Delivery failed: {e.Message}");
            }

            return false;
        }

        /// <summary>
        /// 消费
        /// </summary>
        /// <returns></returns>
        public async Task<T> Consume<T>() where T : class
        {
            return await Task.Run(() =>
             {
                 try
                 {
                     var result = _consumer.Consume(TimeSpan.FromMilliseconds(100));
                     if (result != null)
                     {
                         if (typeof(T) == typeof(string))
                             return (T)Convert.ChangeType(result.Message.Value, typeof(T));

                         return JsonConvert.DeserializeObject<T>(result.Message.Value);
                     }
                     else
                     {
                         Console.WriteLine("null");
                     }
                 }
                 catch (ConsumeException e)
                 {
                     Console.WriteLine($"consume error: {e.Error.Reason}");
                 }
                 catch (Exception e)
                 {
                     Console.WriteLine($"consume error: {e.Message}");
                 }
                 return null;
             });



        }

        public void Subscribe(string topic)
        {
            _consumer.Subscribe(topic);
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            lock (disposeHasBeenCalledLockObj)
            {
                if (disposeHasBeenCalled) { return; }
                disposeHasBeenCalled = true;
            }

            if (disposing)
            {
                _producer?.Dispose();
            }
        }
    }
}

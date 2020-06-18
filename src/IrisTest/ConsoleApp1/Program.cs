using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "192.168.5.25:19092,192.168.5.25:19093,192.168.5.25:19094",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var _c = new KafkaConsumer(conf, "my-topic"))
            {
                while (true)
                {
                    try
                    {

                        var p = _c.Consume<Person>();
                        Console.WriteLine($"Consumed message name:{p.Name}, age:{p.Age} ");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
        }
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
        T Consume<T>() where T : class;
    }

    public class KafkaConsumer : IKafkaConsumer
    {
        private bool disposeHasBeenCalled = false;
        private readonly object disposeHasBeenCalledLockObj = new object();

        private readonly IConsumer<string, string> _consumer;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        /// <summary>
        /// 构造函数，初始化配置
        /// </summary>
        /// <param name="config">配置参数</param>
        /// <param name="topic">主题名称</param>
        public KafkaConsumer(ConsumerConfig config, string topic)
        {
               _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(topic);
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };
        }

        /// <summary>
        /// 消费
        /// </summary>
        /// <returns></returns>
        public T Consume<T>() where T : class
        {
            try
            {
                var result = _consumer.Consume(cts.Token);

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

            return default;
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
                _consumer?.Close();
            }
        }
    }


    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        private readonly KafkaManager _kafkaManager;
        public KafkaController(
            KafkaManager kafkaManager
            )
        {
            _kafkaManager = kafkaManager;
        }

        public async Task<string> Send()
        {
            for (int i = 0; i < 10; ++i)
            {
                var flag = await _kafkaManager.Produce("my-topic", "my-topic", new Person { Name = "flameiris", Age = 18 }, 1);
                if (!flag)
                {
                    Console.WriteLine("send false");
                }
            }
            return "ok";
        }

        public async Task<string> Get()
        {
            List<Person> list = new List<Person>();
            var topic = "my-topic";
            _kafkaManager.Subscribe(topic);
            while (true)
            {
                try
                {
                    var p = await _kafkaManager.Consume<Person>();
                    if (p == null)
                    { break; }
                    list.Add(p);
                    Console.WriteLine($"Consumed message name:{p.Name}, age:{p.Age} ");

                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occured: {e.Error.Reason}");
                }
            }

            return JsonConvert.SerializeObject(list);
        }
    }





    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
using Iris.Models.Model;
using Iris.Models.Model.UserPart;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iris.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var identity = MongoCredential.CreateCredential("iris", "iris", "iris@123");

            //var client = new MongoClient(new MongoClientSettings
            //{
            //    Server = new MongoServerAddress("192.168.5.25", 27018),
            //    //Mongo服务配置中如果配置了security.authorization属性为enabled，则需要认证登录用户; 默认值为disabled，不需要认证。
            //    Credential = new MongoCredential("SCRAM-SHA-1", identity.Identity, identity.Evidence),
            //    MaxConnectionPoolSize = 5,
            //    ClusterConfigurator = onfigur =>
            //    {
            //        onfigur.ConfigureCluster(clusterSetting => clusterSetting.With(serverSelectionTimeout: TimeSpan.FromSeconds(5)));
            //    }
            //});

            //var _db = client.GetDatabase("iris");
            //var _co = _db.GetCollection<User>("user");
            //var users = _co.Find(x => true);

            //Console.WriteLine(users.CountDocuments());




            //var pipeline = PipelineDefinition<User, User>.Create(doc);

            //var list = _co.Aggregate(pipeline).ToList();

            //\u6211



            List<A> AList = new List<A>();
            List<B> BList = new List<B>();
            List<C> CList = new List<C>();

            for (int i = 0; i < 10; i++)
            {
                CList.Add(new C
                {
                    Name = $"FlameIris_C_{i}"
                });
            }
            for (int i = 0; i < 10; i++)
            {
                BList.Add(new B
                {
                    Name = $"FlameIris_B_{i}",
                    CList = CList
                });
            }
            for (int i = 0; i < 10; i++)
            {
                AList.Add(new A
                {
                    Name = $"FlameIris_A_{i}",
                    BList = BList
                });
            }








            Console.ReadKey();

        }
    }

    public class A
    {
        public string Name { get; set; }
        public List<B> BList { get; set; } = new List<B>();

    }

    public class B
    {
        public string Name { get; set; }
        public List<C> CList { get; set; } = new List<C>();

    }

    public class C
    {
        public string Name { get; set; }

    }
}

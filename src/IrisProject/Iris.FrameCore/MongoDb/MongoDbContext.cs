using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace Iris.FrameCore.MongoDb
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database = null;

        public MongoDbContext(IOptions<MongodbOptions> settings)
        {

            var identity = MongoCredential.CreateCredential(settings.Value.Database, settings.Value.Username, settings.Value.Password);

            var client = new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(settings.Value.IP, settings.Value.Port),
                //Mongo服务配置中如果配置了security.authorization属性为enabled，则需要认证登录用户; 默认值为disabled，不需要认证。
                Credential = new MongoCredential("SCRAM-SHA-1", identity.Identity, identity.Evidence),
                MaxConnectionPoolSize = 5,
                WriteConcern = new WriteConcern(),

                ClusterConfigurator = onfigur =>
                {
                    onfigur.ConfigureCluster(clusterSetting => clusterSetting.With(serverSelectionTimeout: TimeSpan.FromSeconds(settings.Value.TimeOutSeconds)));
                }
            });

            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);

        }

        /// <summary>
        /// 根据T获取Collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>() where T : class
        {
            return _database.GetCollection<T>(typeof(T).Name.ToLower());
        }
    }
}

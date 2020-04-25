using Iris.MongoDB;

namespace Iris.Ocelot.Models
{
    public class OcelotConfiguration
    {
        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public string RedisConnectionStrings { get; set; }
        /// <summary>
        /// mongo连接项
        /// </summary>
        public MongodbOptions Mongooptions { get; set; }
        /// <summary>
        /// Mysql连接项
        /// </summary>
        public string MySqlConnectionStrings { get; set; }
        /// <summary>
        /// 默认 0: MongoDB 1：MysqlDB 2：SqlServerDB
        /// </summary>
        //public StorageType StorageType { get; set; } = StorageType.MongoDb;

    }
}

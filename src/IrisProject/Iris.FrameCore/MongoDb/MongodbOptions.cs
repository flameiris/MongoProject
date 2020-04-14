namespace Iris.FrameCore.MongoDb
{
    public class MongodbOptions
    {
        /// <summary>
        /// mongodb数据库IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// mongodb数据库端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// mongodb数据库名称
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// 超时秒数
        /// </summary>
        public int TimeOutSeconds { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}

namespace Iris.FrameCore.RabbitMQ
{
    public class RabbitMqOptions
    {
        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 超时是否自动连接
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; }
        /// <summary>
        /// 延时发送时间-毫秒
        /// </summary>
        public long DelayMillisecond { get; set; }
    }
}

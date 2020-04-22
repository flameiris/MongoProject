using System;

namespace Iris.Models.Request.AgentPart
{
    public class AgentForCreateRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Agentname { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 默认 GUID
        /// </summary>
        public string Salt { get; set; } = Guid.NewGuid().ToString("N");
    }
}

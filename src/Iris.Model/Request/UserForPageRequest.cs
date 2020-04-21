using Iris.Models.Enums;
using System;

namespace Iris.Models.Request
{
    /// <summary>
    /// 请求类
    /// </summary>
    public class UserForPageRequest
    {
        public string Agentname { get; set; }
        public AgentStatusEnum AgentStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


    }
}

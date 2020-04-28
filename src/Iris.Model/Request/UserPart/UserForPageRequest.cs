using Iris.Models.Enums;
using System;

namespace Iris.Models.Request.UserPart
{
    /// <summary>
    /// 请求类
    /// </summary>
    public class UserForPageRequest
    {
        public string Username { get; set; }
        public AgentStatusEnum UserStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


    }
}

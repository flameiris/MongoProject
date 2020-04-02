using AutoMapper;
using Iris.Models.Model;

namespace Iris.Models.Request
{
   
    public class UserForCreateRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
    }
}

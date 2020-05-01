using Iris.Models.Enums;
using System;
using System.Collections.Generic;

namespace Iris.Models.Request.AgentPart
{
    public class AgentForCreateUpdateRequest
    {
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public string Id { get; set; }
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
        /// <summary>
        /// 头像地址
        /// </summary>
        public string ProfilePicture { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 组id集合
        /// </summary>
        public List<string> GroupIdList { get; set; }
        /// <summary>
        /// 角色id集合
        /// </summary>
        public List<string> RoleIdList { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 用户状态，存储在数据库中为 枚举的项
        /// </summary>
        public AgentStatusEnum AgentStatus { get; set; } = AgentStatusEnum.Normal;
        /// <summary>
        /// 用户类型，存储在数据库中为 枚举的项
        /// </summary>
        public AgentTypeEnum AgentType { get; set; } = AgentTypeEnum.Normal;
        /// <summary>
        /// 更新类型
        /// 更新基础数据
        /// 更新角色状态
        /// </summary>
        public int UpdateType { get; set; } = 1;
    }
}

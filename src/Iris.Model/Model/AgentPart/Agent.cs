using Iris.Models.Enums;
using Iris.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Models.Model.AgentPart
{
    public class Agent : IBaseModel
    {
        private DateTime _createTime = DateTime.Now.ToLocalTime();
        private DateTime _updateTime = DateTime.Now.ToLocalTime();

        [Key]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public float Version { get; set; } = 1.0F;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Agentname { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 加密串
        /// </summary>
        public string Salt { get; set; }
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
        [BsonRepresentation(BsonType.String)]
        public AgentStatusEnum AgentStatus { get; set; } = AgentStatusEnum.Normal;
        /// <summary>
        /// 用户类型，存储在数据库中为 枚举的项
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public AgentTypeEnum AgentType { get; set; } = AgentTypeEnum.Normal;
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value.ToLocalTime(); }
        }
        public DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value.ToLocalTime(); }
        }

    }
}

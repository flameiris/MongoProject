using Iris.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Models.Model.UserPart
{
    public class User : IBaseModel
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
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 加密串
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
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
        /// 关联人
        /// </summary>
        public string LinkedPeople { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserBaseinfo Baseinfo { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public short CustomerStatus { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public short CustomerType { get; set; }
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

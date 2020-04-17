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
        public string Username { get; set; }
        public string Password { get; set; }
        public string DigitPassword { get; set; }
        public string Salt { get; set; }
        public UserBaseinfo Baseinfo { get; set; }
        public List<string> GroupIdList { get; set; }
        public List<string> RoleIdList { get; set; }
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

using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Iris.Models.Model
{
    public class User : IBaseModel
    {
        [Key]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public float Version { get; set; } = 1.0F;
        public string Username { get; set; }
        public string Password { get; set; }
        public string DigitPassword { get; set; }
        public string Salt { get; set; }
        public UserBaseinfo Baseinfo { get; set; }
        public List<string> GroupIdList { get; set; }
        public List<string> RoleIdList { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}

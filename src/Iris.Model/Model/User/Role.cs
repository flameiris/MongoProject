using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Iris.Models.Model
{
    public class Role : IBaseModel
    {
        [Key]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public float Version { get; set; } = 1.0F;

        public string Name { get; set; }
        public List<RoleAuth> AuthIdList { get; set; }
    }
}

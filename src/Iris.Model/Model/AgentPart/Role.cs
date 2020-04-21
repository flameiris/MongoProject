using Iris.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Models.Model.AgentPart
{
    public class Role : IBaseModel
    {
        [Key]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public float Version { get; set; } = 1.0F;

        public string Name { get; set; }
        public List<RoleAuth> AuthIdList { get; set; }
    }
}

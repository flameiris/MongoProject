using Iris.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Iris.Models.Model.AgentPart
{
    public class Permission : IBaseModel
    {
        [Key]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public float Version { get; set; } = 1.0F;
        public string Type { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
    }




}

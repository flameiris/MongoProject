using Iris.Models.Enums;
using Iris.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Iris.Models.Model.ContactPart
{
    public class ContactApply : IBaseModel
    {
        private DateTime _createTime = DateTime.Now.ToLocalTime();
        private DateTime _updateTime = DateTime.Now.ToLocalTime();

        [Key]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public float Version { get; set; } = 1.0F;
        public string CustomerId { get; set; }
        public string ApplyCustomerId { get; set; }
        public string Message { get; set; }
        public ContactStatusEnum Status { get; set; }

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

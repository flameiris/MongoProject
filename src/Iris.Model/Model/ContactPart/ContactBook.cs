using Iris.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Iris.Models.Model.ContactPart
{
    /// <summary>
    /// 通讯录
    /// </summary>
    public class ContactBook : IBaseModel
    {
        private DateTime _createTime = DateTime.Now.ToLocalTime();
        private DateTime _updateTime = DateTime.Now.ToLocalTime();

        [Key]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public float Version { get; set; } = 1.0F;
        /// <summary>
        /// 联系人
        /// </summary>
        public List<Contact> ContactList { get; set; }
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
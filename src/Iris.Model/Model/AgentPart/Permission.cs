﻿using Iris.Models.Enums;
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
        /// <summary>
        /// 权限类型
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public PermissionEnum Type { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权限内容
        /// Type为页面权限时，为页面链接 URI
        /// Type为功能权限时，为具体功能
        /// Type为数据权限时，为具体数据所属
        /// </summary>
        public string Content { get; set; }

    }




}

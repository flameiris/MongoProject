using Iris.Models.Enums;
using Iris.Models.Model.AgentPart;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Models.Request.AgentPart
{
    public class PermissionForCreateRequest
    {
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

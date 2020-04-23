using Iris.Models.Model.AgentPart;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Models.Request.AgentPart
{
    public class RoleForCreateRequest
    {
        public string Name { get; set; }
        public List<RolePermission> PermissionIdList { get; set; }
    }
}

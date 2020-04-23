using System.Collections.Generic;

namespace Iris.Models.Model.AgentPart
{
    public class RolePermission
    {
        public int PermissionType { get; set; }
        public List<string> AuthIdList { get; set; }
    }
}

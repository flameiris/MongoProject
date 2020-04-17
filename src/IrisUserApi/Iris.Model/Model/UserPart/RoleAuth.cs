using System.Collections.Generic;

namespace Iris.Models.Model.UserPart
{
    public class RoleAuth
    {
        public int AuthType { get; set; }
        public List<string> AuthIdList { get; set; }
    }
}

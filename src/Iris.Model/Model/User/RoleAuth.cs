using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Models.Model
{
    public class RoleAuth
    {
        public int AuthType { get; set; }
        public List<string> AuthIdList { get; set; }
    }
}

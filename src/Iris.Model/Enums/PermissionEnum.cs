using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Iris.Models.Enums
{
    public enum PermissionEnum
    {
        [Description("菜单权限")]
        Menu = 1,
        [Description("功能权限")]
        Operation = 2,
        [Description("数据权限")]
        Data = 3

    }
}

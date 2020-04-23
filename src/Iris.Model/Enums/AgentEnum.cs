using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Iris.Models.Enums
{

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum AgentTypeEnum
    {
        [Description("匿名用户")]
        Anonymity = 0,
        [Description("普通用户")]
        Normal = 1
    }


    /// <summary>
    /// 用户状态
    /// </summary>
    public enum AgentStatusEnum
    {
        [Description("正常")]
        Normal = 1,
        [Description("锁定")]
        Locked = 2,
    }
}

using System.ComponentModel;

namespace Iris.Models.Enums
{
    public enum BusinessStatusType
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        OK = 200,

        /// <summary>
        /// 参数不符合期望
        /// </summary>
        [Description("参数不符合期望")]
        ParameterUnDesirable = 406
    }
}

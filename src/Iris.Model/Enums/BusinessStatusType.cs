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
        ParameterUnDesirable = 406,

        /// <summary>
        /// 未查询到数据，请稍后重试
        /// </summary>
        [Description("未查询到数据，请稍后重试")]
        NoData = 1,
        /// <summary>
        /// 操作失败，请稍后重试
        /// </summary>
        [Description("操作失败，请稍后重试")]
        OperateError = 2,
    }
}
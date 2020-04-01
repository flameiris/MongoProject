using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Iris.Models.Common
{
    /// <summary>
    /// 通用分页信息类
    /// </summary>
    public class PageModel<TRequest, TResponse>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 10;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; } = 0;
        /// <summary>
        /// 数据总数
        /// </summary>
        public int DataCount { get; set; } = 0;
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 请求数据
        /// </summary>
        public TRequest Request { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<TResponse> Data { get; set; }
    }
}

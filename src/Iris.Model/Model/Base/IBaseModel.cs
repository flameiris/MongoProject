using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Models.Model
{
    /// <summary>
    /// 所有数据表实体类都必须实现此接口
    /// </summary>
    public interface IBaseModel
    {
        string Id { get; set; }
        float Version { get; set; }
    }
}

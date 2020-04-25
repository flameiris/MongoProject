using Iris.Ocelot.OcelotModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Ocelot.Models
{
    /// <summary>
    /// Ocelot配置
    /// </summary>
    public class OcelotConfig
    {
        /// <summary>
        /// 全局配置
        /// </summary>
        public GlobalConfiguration GlobalConfig { get; set; }

        /// <summary>
        /// 路由节点配置
        /// </summary>
        public List<OcelotReRoutes> ReRoutes { get; set; }
    }
}

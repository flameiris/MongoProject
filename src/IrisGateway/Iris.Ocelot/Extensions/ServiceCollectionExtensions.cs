using Iris.MongoDB;
using Iris.Ocelot.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Ocelot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加默认的注入方式，所有需要传入的参数都是用默认值
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IOcelotBuilder AddMongoOcelot(this IOcelotBuilder builder)
        {
            //配置文件仓储注入
            builder.Services.AddSingleton<IFileConfigurationRepository, FileConfigurationRepository>();
            return builder;
        }
    }
}

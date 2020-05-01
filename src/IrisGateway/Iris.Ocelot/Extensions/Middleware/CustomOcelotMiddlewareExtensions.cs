using Microsoft.AspNetCore.Builder;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ocelot.DependencyInjection;
using Ocelot.Configuration.Setter;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.File;
using Ocelot.Responses;
using Ocelot.Configuration;
using Microsoft.AspNetCore.Hosting;
using Ocelot.Logging;
using System.Diagnostics;
using System.Linq;
using Ocelot.Middleware.Pipeline;

namespace Iris.Ocelot.Extensions.Middleware
{
    public static class CustomOcelotMiddlewareExtensions
    {
        public static async Task<IApplicationBuilder> UseCustomOcelot(this IApplicationBuilder builder)
        {
            await builder.UseCustomOcelot(new OcelotPipelineConfiguration());
            return builder;
        }

        public static async Task<IApplicationBuilder> UseCustomOcelot(this IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
        {
            await CreateConfiguration(builder);

            ConfigureDiagnosticListener(builder);

            return CreateOcelotPipeline(builder, pipelineConfiguration);
        }

        private static async Task CreateConfiguration(IApplicationBuilder builder)
        {
            //重写从数据库中获取
            var fileConfig = await builder.ApplicationServices.GetService<IFileConfigurationRepository>().Get();
            var internalConfigCreator = builder.ApplicationServices.GetService<IInternalConfigurationCreator>();
            var internalConfig = await internalConfigCreator.Create(fileConfig.Data);
            //如果配置文件错误直接抛出异常
            if (internalConfig.IsError)
            {
                ThrowToStopOcelotStarting(internalConfig);
            }

            //配置信息缓存分布式架构(Redis) 未实现 （暂时通过轮询方式获取改变）
            var internalConfigRepo = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();
            internalConfigRepo.AddOrReplace(internalConfig.Data);

            var adminPath = builder.ApplicationServices.GetService<IAdministrationPath>();

            var configurations = builder.ApplicationServices.GetServices<OcelotMiddlewareConfigurationDelegate>();

            // Todo - this has just been added for consul so far...will there be an ordering problem in the future? Should refactor all config into this pattern?
            foreach (var configuration in configurations)
            {
                await configuration(builder);
            }

            //查询Redis是否配置有变化 TODO
            //GetConfigByLoop(builder, internalConfigCreator, internalConfigRepo);
            //return GetOcelotConfigAndReturn(internalConfigRepo);
        }
        private static IApplicationBuilder CreateOcelotPipeline(IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
        {
            var pipelineBuilder = new OcelotPipelineBuilder(builder.ApplicationServices);

            //重写扩展
            pipelineBuilder.BuildCustomOcelotPipeline(pipelineConfiguration);

            var firstDelegate = pipelineBuilder.Build();

            /*
            inject first delegate into first piece of asp.net middleware..maybe not like this
            then because we are updating the http context in ocelot it comes out correct for
            rest of asp.net..
            */

            builder.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";

            builder.Use(async (context, task) =>
            {
                var downstreamContext = new DownstreamContext(context);
                await firstDelegate.Invoke(downstreamContext);
            });

            return builder;
        }


        /// <summary>
        /// 检测更改
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="internalConfigCreator"></param>
        /// <param name="internalConfigRepo"></param>
        /// <returns></returns>
        //private static async Task GetConfigByLoop(IApplicationBuilder builder, IInternalConfigurationCreator internalConfigCreator, IInternalConfigurationRepository internalConfigRepo)
        //{
        //    while (true)
        //    {
        //        var IsUpdate = await RedisHelper.GetAsync(RedisKeys.Ocelot_Configuration_Update);
        //        if (!string.IsNullOrWhiteSpace(IsUpdate))
        //        {
        //            var fileConfig = await builder.ApplicationServices.GetService<IFileConfigurationRepository>().Get();
        //            var newInternalConfig = await internalConfigCreator.Create(fileConfig.Data);
        //            if (newInternalConfig.IsError)
        //            {
        //                ThrowToStopOcelotStarting(newInternalConfig);
        //            }
        //            internalConfigRepo.AddOrReplace(newInternalConfig.Data);
        //            if (RedisHelper.Exists(RedisKeys.Ocelot_Configuration_Update))
        //            {
        //                await RedisHelper.DelAsync(RedisKeys.Ocelot_Configuration_Update);
        //            }
        //        }
        //        System.Threading.Thread.Sleep(10 * 60 * 1000);
        //    }
        //}

        private static bool AdministrationApiInUse(IAdministrationPath adminPath)
        {
            return adminPath != null;
        }

        private static async Task SetFileConfig(IFileConfigurationSetter fileConfigSetter, IOptionsMonitor<FileConfiguration> fileConfig)
        {
            var response = await fileConfigSetter.Set(fileConfig.CurrentValue);

            if (IsError(response))
            {
                ThrowToStopOcelotStarting(response);
            }
        }

        private static bool IsError(Response response)
        {
            return response == null || response.IsError;
        }

        private static IInternalConfiguration GetOcelotConfigAndReturn(IInternalConfigurationRepository provider)
        {
            var ocelotConfiguration = provider.Get();

            if (ocelotConfiguration?.Data == null || ocelotConfiguration.IsError)
            {
                ThrowToStopOcelotStarting(ocelotConfiguration);
            }

            return ocelotConfiguration.Data;
        }

        private static void ThrowToStopOcelotStarting(Response config)
        {
            throw new Exception($"Unable to start Ocelot, errors are: {string.Join(",", config.Errors.Select(x => x.ToString()))}");
        }

        private static void ConfigureDiagnosticListener(IApplicationBuilder builder)
        {
            var env = builder.ApplicationServices.GetService<IHostingEnvironment>();
            var listener = builder.ApplicationServices.GetService<OcelotDiagnosticListener>();
            var diagnosticListener = builder.ApplicationServices.GetService<DiagnosticListener>();
            diagnosticListener.SubscribeWithAdapter(listener);
        }
    }
}

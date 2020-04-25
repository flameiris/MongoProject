using Iris.Infrastructure.ExtensionMethods;
using Iris.MongoDB;
using Iris.Ocelot.OcelotModels;
using Mapster;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Ocelot.Repository
{
    public class FileConfigurationRepository : IFileConfigurationRepository
    {
        private readonly IMongoDbManager<GlobalConfiguration> _globalMongo;
        private readonly IMongoDbManager<OcelotReRoutes> _reRouteMongo;

        public FileConfigurationRepository(
            IMongoDbManager<GlobalConfiguration> globalMongo,
            IMongoDbManager<OcelotReRoutes> reRouteMongo
            )
        {
            _globalMongo = globalMongo;
            _reRouteMongo = reRouteMongo;
        }
        public async Task<Response<FileConfiguration>> Get()
        {
            var g = await _globalMongo.GetFirstOrDefaultAsync();
            var r = (await _reRouteMongo.GetAsync()).ToList();
            var file = new FileConfiguration();
            var glb = new FileGlobalConfiguration();

            if (g == null)
                throw new Exception("未获取到全局配置信息");

            if (r == null || !r.Any())
                throw new Exception("未获取到路由配置信息");


            //赋值全局配置
            glb.BaseUrl = g.BaseUrl;
            glb.DownstreamScheme = g.DownstreamScheme;
            glb.RequestIdKey = g.RequestIdKey;
            glb.RateLimitOptions = g.RateLimitOptions?.Adapt<FileRateLimitOptions>();
            glb.HttpHandlerOptions = g.HttpHandlerOptions?.Adapt<FileHttpHandlerOptions>();
            glb.LoadBalancerOptions = g.LoadBalancerOptions?.Adapt<FileLoadBalancerOptions>();
            glb.QoSOptions = g.QoSOptions?.Adapt<FileQoSOptions>();
            glb.ServiceDiscoveryProvider = g.ServiceDiscoveryProvider?.Adapt<FileServiceDiscoveryProvider>();
            file.GlobalConfiguration = glb;

            //赋值路由配置
            var reroutelist = new List<FileReRoute>();
            foreach (var o in r)
            {
                //路由节点配置
                var m = new FileReRoute();
                m.FileCacheOptions = o.CacheOptions?.Adapt<FileCacheOptions>();
                m.AuthenticationOptions = o.AuthenticationOptions?.Adapt<FileAuthenticationOptions>();
                m.LoadBalancerOptions = o.LoadBalancerOptions?.Adapt<FileLoadBalancerOptions>();
                m.QoSOptions = o.QoSOptions?.Adapt<FileQoSOptions>();
                m.RateLimitOptions = o.RateLimitOptions?.Adapt<FileRateLimitRule>();
                m.DelegatingHandlers = o.DelegatingHandlers;

                m.UpstreamHost = o.UpstreamHost;
                m.UpstreamHttpMethod = o.UpstreamHttpMethod;
                m.UpstreamPathTemplate = o.UpstreamPathTemplate;
                m.DownstreamPathTemplate = o.DownstreamPathTemplate;
                m.DownstreamScheme = o.DownstreamScheme;
                if (o.DownstreamHostAndPorts != null)
                {
                    var hap = new List<FileHostAndPort>();
                    o.DownstreamHostAndPorts.ForEach(x =>
                    {
                        hap.Add(new FileHostAndPort()
                        {
                            Host = x.Host,
                            Port = x.Port
                        });
                    });
                    m.DownstreamHostAndPorts = hap;
                }

                m.Priority = o.Priority;
                m.RequestIdKey = o.RequestIdKey;
                m.ServiceName = o.ServiceName;

                m.ReRouteIsCaseSensitive = o.ReRouteIsCaseSensitive;
                m.RouteClaimsRequirement = o.RouteClaimsRequirement;
                reroutelist.Add(m);
            }
            file.ReRoutes = reroutelist;


            if (file.ReRoutes == null || file.ReRoutes.Count == 0)
            {
                return new OkResponse<FileConfiguration>(null);
            }
            return new OkResponse<FileConfiguration>(file);
        }

        public async Task<Response> Set(FileConfiguration fileConfiguration)
        {
            return new OkResponse();
        }
    }
}

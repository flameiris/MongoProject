using Iris.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iris.Ocelot.OcelotModels
{
    //public class OcelotReRoutesRoots : IBaseModel
    //{
    //    /// <summary>
    //    /// Id编号
    //    /// </summary>
    //    /// <value></value>
    //    [Key]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string Id { get; set; }
    //    public float Version { get; set; } = 1.0F;
    //    public string Key { get; set; }
    //    public string OcelotListenPort { get; set; }
    //    public List<OcelotReRoutes> ReRoutes { get; set; }

    //}


    public class OcelotReRoutes : IBaseModel
    {
        /// <summary>
        /// Id编号
        /// </summary>
        /// <value></value>
        [Key]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public float Version { get; set; } = 1.0F;
        public string Key { get; set; }
        /// <summary>
        /// 认证配置
        /// </summary>
        public OcelotAuthenticationOptions AuthenticationOptions { get; set; }

        /// <summary>
        /// 以对下游请求结果进行缓存，主要依赖于CacheManager实现 JSON
        /// </summary>
        public OcelotCacheOptions CacheOptions { get; set; }

        /// <summary>
        /// GraphiQL是一款内置在浏览器中的GraphQL探索工具
        /// </summary>
        public List<string> DelegatingHandlers { get; set; }

        /// <summary>
        /// 负载均衡配置 JSON
        /// </summary>
        public OcelotLoadBalancerOptions LoadBalancerOptions { get; set; }

        /// <summary>
        /// 熔断配置,停止将请求转发到下游服务 JSON
        /// </summary>
        public OcelotQoSOptions QoSOptions { get; set; }

        /// <summary>
        /// 下游服务端口号和地址 Json
        /// </summary>
        public List<OcelotHostAndPort> DownstreamHostAndPorts { get; set; }

        /// <summary>
        /// 下游服务URL模板
        /// </summary>
        public string DownstreamPathTemplate { get; set; }

        /// <summary>
        /// 下游服务schema：http, https
        /// </summary>
        public string DownstreamScheme { get; set; }

        /// <summary>
        /// 优先级 当我们配置多个请求产生冲突的时候，通过路由设置访问优化级  
        /// </summary>
        public int Priority { get; set; } = 0;

        public string RequestIdKey { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务描述
        /// </summary>
        /// <value></value>
        public string ServiceDescription { get; set; }

        /// <summary>
        /// 上游服务Ip地址
        /// </summary>
        public string UpstreamHost { get; set; }

        /// <summary>
        /// 上游方法类型Get,Post,Put
        /// </summary>
        public List<string> UpstreamHttpMethod { get; set; }

        /// <summary>
        /// 上游服务URL模板
        /// </summary>
        public string UpstreamPathTemplate { get; set; }

        /// <summary>
        /// 需要在转发过程中添加到Header的内容
        /// </summary>
        public Dictionary<string, string> AddHeadersToRequest { get; set; }

        /// <summary>
        /// 重写路由是否区分大小写
        /// </summary>
        public bool ReRouteIsCaseSensitive { get; set; } = false;

        /// <summary>
        /// 限流设置 JSON
        /// </summary>
        public OcelotRateLimitRule RateLimitOptions { get; set; }

        /// <summary>
        /// 标记该路由是否需要认证
        /// registered 示例,K/V形式，授权声明，授权token中会包含一些claim，如填写则会判断是否和token中的一致，不一致则不准访问
        /// </summary>
        public Dictionary<string, string> RouteClaimsRequirement { get; set; }

        public Boolean UseServiceDiscovery { get; set; }
    }
}
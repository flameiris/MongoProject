using System;
using System.ComponentModel.DataAnnotations;
using Iris.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Iris.Ocelot.OcelotModels
{
    public class GlobalConfiguration : IBaseModel
    {
        [Key]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public float Version { get; set; }
        /// <summary>
        /// 全局请求默认key
        /// </summary>
        public string RequestIdKey { get; set; }

        /// <summary>
        /// 网关基础地质
        /// </summary>
        public string BaseUrl { get; set; }

        public OcelotAuthorityOptions AuthorityOptions { get; set; }

        /// <summary>
        /// 下游服务schema：http, https
        /// </summary>
        public string DownstreamScheme { get; set; }

        /// <summary>
        /// 服务发现全局配置,值为配置json
        /// </summary>
        public OcelotServiceDiscoveryProvider ServiceDiscoveryProvider { get; set; }

        /// <summary>
        /// 全局负载均衡配置
        /// </summary>
        public OcelotLoadBalancerOptions LoadBalancerOptions { get; set; }

        /// <summary>
        /// Http请求配置
        /// </summary>
        public OcelotHttpHandlerOptions HttpHandlerOptions { get; set; }

        /// <summary>
        /// 服务质量配置（熔断）停止将请求转发到下游服务（JSON）
        /// </summary>
        public OcelotQoSOptions QoSOptions { get; set; }

        /// <summary>
        /// 全局设置限流措施（JSON）
        /// </summary>
        public OcelotRateLimitOptions RateLimitOptions { get; set; }
    }
}
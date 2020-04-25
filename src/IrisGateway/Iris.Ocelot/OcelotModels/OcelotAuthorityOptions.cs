namespace Iris.Ocelot.OcelotModels
{
    public class OcelotAuthorityOptions
    {
        /// <summary>
        /// 是否启用身份验证
        /// </summary>
        /// <value></value>
        public bool IsAuthority { get; set; }

        /// <summary>
        /// 认证服务器地址
        /// </summary>
        /// <value></value>
        public string Authority { get; set; }
    }
}
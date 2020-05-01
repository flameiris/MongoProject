using Iris.FrameCore;
using Iris.Infrastructure.ExtensionMethods;
using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Enums;
using Iris.Models.Model.AgentPart;
using Iris.Models.Request.AgentPart;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Iris.Identity.Service
{
    public class AgentService : IAgentService
    {
        private readonly string _url = "http://localhost:10002/";
        private readonly HttpWebClient _client;
        public AgentService(
            HttpWebClient client
            )
        {
            _client = client;
        }

        public async Task<dynamic> GetAgentByNameAndPwd(AgentForIdCenterRequest r)
        {
            var requestUrl = "agentapiservice/getagentbynameandpwd";
            var res = await _client.PostJsonAsync(_url + requestUrl, JsonConvert.SerializeObject(r));
            if (res.code != HttpStatusCode.OK)
            {
                throw new Exception($"{res.code }  {res.ret}");
            }
            return (dynamic)JObject.Parse(res.ret);
        }
    }
}

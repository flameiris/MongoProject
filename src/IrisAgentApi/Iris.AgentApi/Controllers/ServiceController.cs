using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iris.AgentApi.Controllers.Base;
using Iris.Models.Common;
using Iris.Models.Model.AgentPart;
using Iris.Models.Request;
using Iris.Models.Request.AgentPart;
using Iris.MongoDB;
using Iris.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iris.AgentApi.Controllers
{
    [ApiController]
    [Route("agentapiservice/[action]")]
    public class ServiceController : ControllerBase
    {
        private readonly IAgentService _agentService;
        public ServiceController(
            IAgentService agentService
            )
        {
            _agentService = agentService;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> GetAgentByNameAndPwd(AgentForIdCenterRequest request)
        {
            Thread.Sleep(10000);
            return await _agentService.GetAgentByNameAndPwd(request);
        }
    }
}
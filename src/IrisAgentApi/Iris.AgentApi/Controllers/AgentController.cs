﻿using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Request;
using Iris.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Iris.AgentApi.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;
        public AgentController(
            IAgentService agentService
            )
        {
            _agentService = agentService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Create(AgentForCreateRequest request)
        {
            return await _agentService.Create(request);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse> GetDetail(string id)
        {
            return await _agentService.GetDetail(id);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageModel">分页对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> GetListByPage(PageModel<AgentForPageRequest, AgentForListDto> pageModel)
        {
            var res = await _agentService.GetListByPage(pageModel);
            return res;
        }
    }
}
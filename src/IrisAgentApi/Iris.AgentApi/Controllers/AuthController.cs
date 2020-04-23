using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iris.AgentApi.Controllers.Base;
using Iris.Models.Common;
using Iris.Models.Dto.AgentPart;
using Iris.Models.Request.AgentPart;
using Iris.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iris.AgentApi.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(
            IAuthService authService
            )
        {
            _authService = authService;
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Create(RoleForCreateRequest r)
        {
            return await _authService.Create(r);
        }

        /// <summary>
        /// 查询角色详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse> GetDetail(string id)
        {
            return await _authService.GetDetail(id);
        }

        /// <summary>
        /// 分页查询角色
        /// </summary>
        /// <param name="pageModel">分页对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> GetListByPage(PageModel<RoleForPageRequest, RoleForListDto> p)
        {
            var res = await _authService.GetListByPage(p);
            return res;
        }



        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> CreatePermission(PermissionForCreateRequest r)
        {
            return await _authService.CreatePermission(r);
        }

        /// <summary>
        /// 查询角色详情
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse> GetPermissionDetail(string id)
        {
            return await _authService.GetPermissionDetail(id);
        }

        /// <summary>
        /// 查询所有权限
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> GetPermissionList(RoleForPageRequest p)
        {
            return await _authService.GetPermissionList(p);
        }

    }
}
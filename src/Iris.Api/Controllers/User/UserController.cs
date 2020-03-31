using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iris.Models.Request;
using Iris.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(
            IUserService userService
            )
        {
            _userService = userService;
        }



        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> Create(UserForCreateRequest request)
        {
            return await _userService.Create(request);
        }
    }
}
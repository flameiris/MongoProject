using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iris.FrameCore.MongoDb;
using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Model;
using Iris.Models.Request;
using Iris.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Api.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IMongoDbManager<User> _userMongo;
        public UserController(
            IUserService userService,
            IMongoDbManager<User> userMongo
            )
        {
            _userService = userService;
            _userMongo = userMongo;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Create(UserForCreateRequest request)
        {
            return await _userService.Create(request);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageModel">分页对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> GetUserListByPage(PageModel<UserForPageRequest, UserForListDto> pageModel)
        {
            var res = await _userService.GetUserListByPage(pageModel);
            res.Response = pageModel;
            return res;
        }
    }
}
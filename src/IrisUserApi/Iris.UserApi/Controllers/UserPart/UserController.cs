using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Request;
using Iris.Service.IService.UserPart;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Iris.UserApi.Controllers.UserPart
{
    /// <summary>
    /// 用户控制器
    /// </summary>
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
        [HttpPost]
        public async Task<BaseResponse> Create(UserForCreateRequest request)
        {
            return await _userService.Create(request);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse> GetUserDetail(string userId)
        {
            return await _userService.GetUserDetail(userId);
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
            return res;
        }
    }
}
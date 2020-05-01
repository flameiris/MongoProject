using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Dto.UserPart;
using Iris.Models.Request;
using Iris.Models.Request.UserPart;
using System.Threading.Tasks;

namespace Iris.Service.IService.UserPart
{
    public interface IUserService
    {
        Task<BaseResponse> Create(UserForCreateRequest request);
        Task<BaseResponse> GetUserDetail(string userId);
        Task<BaseResponse> GetUserListByPage(PageModel<UserForPageRequest, UserForListDto> pageModel);
    }
}

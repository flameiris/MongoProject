using Iris.Models.Common;
using Iris.Models.Dto.AgentPart;
using Iris.Models.Model.AgentPart;
using Iris.Models.Request.AgentPart;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Service.IService
{
    public interface IAuthService
    {
        Task<BaseResponse> Create(RoleForCreateRequest r);
        Task<BaseResponse> GetDetail(string id);
        Task<List<Role>> GetListByIdList(List<string> idList);
        Task<BaseResponse> GetListByPage(PageModel<RoleForPageRequest, RoleForListDto> p);
        Task<BaseResponse> CreatePermission(PermissionForCreateRequest r);
        Task<BaseResponse> GetPermissionDetail(string id);
        Task<BaseResponse> GetPermissionList(RoleForPageRequest p);
    }
}

using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Request;
using Iris.Models.Request.AgentPart;
using System.Threading.Tasks;

namespace Iris.Service.IService
{
    public interface IAgentService
    {
        Task<BaseResponse> Create(AgentForCreateRequest request);
        Task<BaseResponse> GetDetail(string id);
        Task<BaseResponse> GetListByPage(PageModel<AgentForPageRequest, AgentForListDto> pageModel);
        Task<BaseResponse> GetAgentByNameAndPwd(AgentForIdCenterRequest r);
    }
}

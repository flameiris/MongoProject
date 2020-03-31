using Iris.Models.Request;
using System.Threading.Tasks;

namespace Iris.Service.IService
{
    public interface IUserService
    {
        Task<bool> Create(UserForCreateRequest request);
    }
}

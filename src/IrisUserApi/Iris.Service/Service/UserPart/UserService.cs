using AutoMapper;
using Iris.Infrastructure.ExtensionMethods;
using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Enums;
using Iris.Models.Model.UserPart;
using Iris.Models.Request;
using Iris.MongoDB;
using Iris.Service.IService.UserPart;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Iris.Service.Service.UserPart
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IMongoDbManager<User> _userMongo;

        public UserService(
            IMapper mapper,
            IMongoDbManager<User> userMongo
            )
        {
            _mapper = mapper;
            _userMongo = userMongo;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse> Create(UserForCreateRequest request)
        {
            User user = new User
            {
                Username = request.Username,
                Password = request.Password

            };

            var flag = await _userMongo.AddAsync(user);
            if (!flag) return BaseResponse.GetBaseResponse(BusinessStatusType.OperateError);
            return BaseResponse.GetBaseResponse(BusinessStatusType.OK);


        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetUserDetail(string userId)
        {
            var user = await _userMongo.GetFirstOrDefaultAsync(x => x.Id == userId && x.Version == 1.0);
            if (user == null) return BaseResponse.GetBaseResponse(BusinessStatusType.NoData);



            var model = UserForDetailDto.MapTo(user);
            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, model);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetUserListByPage(PageModel<UserForPageRequest, UserForListDto> pageModel)
        {
            var r = pageModel.Request;
            MongoModel<User> mongo = new MongoModel<User>();
            mongo.Ascending(pageModel.Sort);
            if (r.Username.EnabledStr()) mongo.Where(x => x.Username == r.Username || x.Username == "flame2");
            if (r.StartTime.EnabledDateTime() && r.EndTime.EnabledDateTime())
                mongo.Or(x => x.CreateTime > r.StartTime && x.CreateTime < r.EndTime);
            mongo.Exclude("Baseinfo.DeliveryAddressList");


            //根据条件查询记录数
            var count = await _userMongo.CountAsync(mongo.Filter.And(mongo.FilterList));

            //分页查询
            var list = (await _userMongo.GetByPageAsync(mongo, pageModel.PageIndex, pageModel.PageSize)).ToList();
            if (!list.Any())
                return BaseResponse.GetBaseResponse(BusinessStatusType.NoData, "未查询到数据，请稍后重试");

            var dtoList = list.Select(x => UserForListDto.MapTo(x)).ToList();



            pageModel.Data = dtoList;
            pageModel.DataCount = count;

            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, pageModel);
        }
    }
}

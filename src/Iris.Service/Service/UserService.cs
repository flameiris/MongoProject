using AutoMapper;
using Iris.FrameCore.MongoDb;
using Iris.Infrastructure.ExtensionMethods;
using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Enums;
using Iris.Models.Model;
using Iris.Models.Request;
using Iris.Service.IService;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Service.Service
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


            var model = UserForDetailDto.MapTo(_mapper, user);
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
            mongo.Include("Username", "Password");


            //根据条件查询记录数
            var count = await _userMongo.CountAsync(mongo.Filter.And(mongo.FilterList));

            //分页查询
            var list = (await _userMongo.GetByPageAsync(mongo, pageModel.PageIndex, pageModel.PageSize)).ToList();
            if (!list.Any())
                return BaseResponse.GetBaseResponse(BusinessStatusType.NoData, "未查询到数据，请稍后重试");

            pageModel.Data = list.Select(x => UserForListDto.Map(_mapper, x)).ToList();
            pageModel.DataCount = count;

            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, pageModel);
        }


    }
}

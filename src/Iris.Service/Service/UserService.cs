using AutoMapper;
using Iris.FrameCore.MongoDb;
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
        public async Task<UserForDetailDto> GetUserDetail(string id)
        {
            var user = await _userMongo.GetFirstOrDefaultAsync(x => x.Id == id && x.Version == 1.0);
            return _mapper.Map<UserForDetailDto>(user);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetUserListByPage(PageModel<UserForPageRequest, UserForListDto> pageModel)
        {
            MongoModel<User> mongoModel = new MongoModel<User>();
            mongoModel.SortList.Add(mongoModel.Sort.Ascending(pageModel.Sort));
            mongoModel.FilterList.Add(mongoModel.Filter.Where(x => x.Username == pageModel.Request.Username));

            //根据条件查询记录数
            var count = await _userMongo.CountAsync(null);

            //分页查询
            var list = (await _userMongo.GetByPageAsync(mongoModel, pageModel.PageIndex, pageModel.PageSize)).ToList();
            if (!list.Any())
                return BaseResponse.GetBaseResponse(BusinessStatusType.NoData, "未查询到数据，请稍后重试");

            pageModel.Data = list.Select(x => _mapper.Map<UserForListDto>(x)).ToList();

            return BaseResponse.GetBaseResponse(BusinessStatusType.OK);
        }


    }
}

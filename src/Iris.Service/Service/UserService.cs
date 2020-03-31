using AutoMapper;
using Iris.FrameCore.MongoDb;
using Iris.Models.Dto;
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
        public async Task<bool> Create(UserForCreateRequest request)
        {
            User user = new User
            {
                Username = request.Username,
                Password = request.Password
            };

            return await _userMongo.AddAsync(user);
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
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<UserForListDto>> GetUserListByPage(UserForPageRequest request)
        {

            List<UserForListDto> res = new List<UserForListDto>();
            //排序字段
            var sort = Builders<User>.Sort.Descending("Createtime");

            //组装查询条件
            var filterList = new List<FilterDefinition<User>>();

            var filter = filterList.Any() ? Builders<User>.Filter.And(filterList) : null;

            //根据条件查询记录数
            var count = await _userMongo.CountAsync(filter);

            //分页查询
            var list = (await _userMongo.GetByPageAsync(filter, request.PageIndex, request.PageSize, sort)).ToList();
            if (!list.Any())
                return res;

            res.AddRange(list.Select(x => _mapper.Map<UserForListDto>(x)));

            return res;
        }



    }
}

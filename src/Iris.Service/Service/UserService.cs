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
        private readonly IMongoDbManager<User> _userMongo;

        public async Task<bool> Create(User request)
        {
            return await _userMongo.AddAsync(request);
        }

        public async Task<List<UserForListDto>> GetUserListByPage(UserForPageRequest request)
        {

            List<UserForListDto> res = new List<UserForListDto>();

            //排序字段
            var sort = Builders<User>.Sort.Descending("Createtime");

            //组装查询条件
            var filterList = new List<FilterDefinition<User>>();



            var filter = filterList.Any() ? Builders<User>.Filter.And(filterList) : null;

            //根据条件查询记录数
            //var count = await _UserMongo.CountAsync(filter);

            //分页查询
            //var list = (await _UserMongo.GetByPage(filter, request.PageIndex, request.PageSize, sort)).ToList();
            //if (!list.Any())
            //    return res;

            //list.ForEach(x =>
            //{
            //    res.Add(new UserForListDto
            //    {
            //    });
            //});

            return res;
        }

        public async Task<UserForDetailDto> GetUserDetail(string id)
        {
            var user = await _userMongo.GetAsync(x => x.Id == id && x.Version == 1.0);
            return new UserForDetailDto();
        }



    }
}

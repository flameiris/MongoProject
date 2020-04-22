using AutoMapper;
using Iris.Infrastructure.ExtensionMethods;
using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Enums;
using Iris.Models.Model.AgentPart;
using Iris.Models.Request;
using Iris.Models.Request.AgentPart;
using Iris.MongoDB;
using Iris.Service.IService;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Iris.Service.Service
{
    public class AgentService : IAgentService
    {
        private readonly IMapper _mapper;
        private readonly IMongoDbManager<Agent> _agentMongo;

        public AgentService(
            IMapper mapper,
            IMongoDbManager<Agent> agentMongo
            )
        {
            _mapper = mapper;
            _agentMongo = agentMongo;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse> Create(AgentForCreateRequest request)
        {
            Agent agent = new Agent
            {
                Agentname = request.Agentname,
                Password = (request.Password + request.Salt).MD5(),
                Salt = request.Salt,

            };

            var flag = await _agentMongo.AddAsync(agent);
            if (!flag) return BaseResponse.GetBaseResponse(BusinessStatusType.OperateError);
            return BaseResponse.GetBaseResponse(BusinessStatusType.OK);


        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetDetail(string id)
        {
            var agent = await _agentMongo.GetFirstOrDefaultAsync(x => x.Id == id && x.Version == 1.0);
            if (agent == null) return BaseResponse.GetBaseResponse(BusinessStatusType.NoData);
            var dto = AgentForDetailDto.MapTo(agent);
            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, dto);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetListByPage(PageModel<AgentForPageRequest, AgentForListDto> pageModel)
        {
            var r = pageModel.Request;
            MongoModel<Agent> mongo = new MongoModel<Agent>();
            mongo.Ascending(pageModel.Sort);
            if (r.Agentname.EnabledStr()) mongo.Where(x => x.Agentname == r.Agentname || x.Agentname == "flame2");
            if (r.StartTime.EnabledDateTime() && r.EndTime.EnabledDateTime())
                mongo.Or(x => x.CreateTime > r.StartTime && x.CreateTime < r.EndTime);
            //mongo.Exclude("Baseinfo.DeliveryAddressList");


            //根据条件查询记录数
            var count = await _agentMongo.CountAsync(mongo.Filter.And(mongo.FilterList));

            //分页查询
            var list = (await _agentMongo.GetByPageAsync(mongo, pageModel.PageIndex, pageModel.PageSize)).ToList();
            if (!list.Any())
                return BaseResponse.GetBaseResponse(BusinessStatusType.NoData, "未查询到数据，请稍后重试");

            var dtoList = list.Select(x => AgentForListDto.MapTo(x)).ToList();



            pageModel.Data = dtoList;
            pageModel.DataCount = count;

            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, pageModel);
        }

        /// <summary>
        /// 根据用户名密码获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetAgentByNameAndPwd(AgentForIdCenterRequest r)
        {
            var agent = await _agentMongo.GetFirstOrDefaultAsync(x => x.Agentname == r.Name && x.Version == 1.0);
            if (agent == null) return BaseResponse.GetBaseResponse(BusinessStatusType.NoData);

            var md5Pwd = (r.Password + agent.Salt).MD5();
            agent = await _agentMongo.GetFirstOrDefaultAsync(x => x.Agentname == r.Name && x.Password == md5Pwd && x.Version == 1.0);
            if (agent == null) return BaseResponse.GetBaseResponse(BusinessStatusType.NoData);


            var dto = AgentForDetailDto.MapTo(agent);
            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, dto);
        }
    }
}

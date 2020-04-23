using Iris.Infrastructure.ExtensionMethods;
using Iris.Models.Common;
using Iris.Models.Dto;
using Iris.Models.Dto.AgentPart;
using Iris.Models.Enums;
using Iris.Models.Model.AgentPart;
using Iris.Models.Request;
using Iris.Models.Request.AgentPart;
using Iris.MongoDB;
using Iris.Service.IService;
using Mapster;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Iris.Service.Service
{
    public class AgentService : IAgentService
    {
        private readonly IAuthService _authService;

        private readonly IMongoDbManager<Agent> _agentMongo;

        public AgentService(
            IAuthService authService,
            IMongoDbManager<Agent> agentMongo
            )
        {
            _authService = authService;
            _agentMongo = agentMongo;

        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public async Task<BaseResponse> Create(AgentForCreateUpdateRequest r)
        {
            Agent agent = new Agent
            {
                Agentname = r.Agentname,
                Password = (r.Password + r.Salt).MD5(),
                Salt = r.Salt,

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

            var roleList = await _authService.GetListByIdList(agent.RoleIdList);


            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, new
            {
                Agent = agent,
                RoleList = roleList
            });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetListByPage(PageModel<AgentForPageRequest, AgentForListDto> p)
        {
            var r = p.Request;
            MongoModel<Agent> mongo = new MongoModel<Agent>();
            mongo.Ascending(p.Sort);
            if (r.Agentname.EnabledStr()) mongo.Where(x => x.Agentname == r.Agentname || x.Agentname == "flame2");
            if (r.StartTime.EnabledDateTime() && r.EndTime.EnabledDateTime())
                mongo.Or(x => x.CreateTime > r.StartTime && x.CreateTime < r.EndTime);
            //mongo.Exclude("Baseinfo.DeliveryAddressList");


            //根据条件查询记录数
            var count = await _agentMongo.CountAsync(mongo.Filter.And(mongo.FilterList));

            //分页查询
            var list = (await _agentMongo.GetByPageAsync(mongo, p.PageIndex, p.PageSize)).ToList();
            if (!list.Any())
                return BaseResponse.GetBaseResponse(BusinessStatusType.NoData, "未查询到数据，请稍后重试");

            var dtoList = list.Select(x => AgentForListDto.MapTo(x)).ToList();



            p.Data = dtoList;
            p.DataCount = count;

            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, p);
        }


        public async Task<BaseResponse> UpdateDetail(AgentForCreateUpdateRequest r)
        {
            MongoModel<Agent> mongo = new MongoModel<Agent>();
            mongo.Where(x => x.Id == r.Id);
            return null;
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

            if (agent.AgentStatus == AgentStatusEnum.Locked)
                return BaseResponse.GetBaseResponse(BusinessStatusType.NoData);

            var md5Pwd = (r.Password + agent.Salt).MD5();
            if (!agent.Password.Equals(md5Pwd))
                return BaseResponse.GetBaseResponse(BusinessStatusType.NoData);

            var dto = agent.Adapt<AgentForDetailDto>();
            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, dto);
        }
    }
}

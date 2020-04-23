using AutoMapper;
using Iris.Infrastructure.ExtensionMethods;
using Iris.Models.Common;
using Iris.Models.Dto.AgentPart;
using Iris.Models.Enums;
using Iris.Models.Model.AgentPart;
using Iris.Models.Request.AgentPart;
using Iris.MongoDB;
using Iris.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IMongoDbManager<Agent> _agentMongo;
        private readonly IMongoDbManager<Group> _groupMongo;
        private readonly IMongoDbManager<Role> _roleMongo;
        private readonly IMongoDbManager<Permission> _permissionMongo;


        public AuthService(
            IMapper mapper,
            IMongoDbManager<Agent> agentMongo,
            IMongoDbManager<Group> groupMongo,
            IMongoDbManager<Role> roleMongo,
            IMongoDbManager<Permission> permissionMongo

            )
        {
            _mapper = mapper;
            _agentMongo = agentMongo;
            _groupMongo = groupMongo;
            _roleMongo = roleMongo;
            _permissionMongo = permissionMongo;
        }


        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse> Create(RoleForCreateRequest r)
        {
            var role = new Role
            {
                Name = r.Name,
                PermissionIdList = r.PermissionIdList

            };

            var flag = await _roleMongo.AddAsync(role);
            if (!flag) return BaseResponse.GetBaseResponse(BusinessStatusType.OperateError);
            return BaseResponse.GetBaseResponse(BusinessStatusType.OK);
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetDetail(string id)
        {
            var role = await _roleMongo.GetFirstOrDefaultAsync(x => x.Id == id && x.Version == 1.0);
            if (role == null) return BaseResponse.GetBaseResponse(BusinessStatusType.NoData);

            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, role);
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Role>> GetListByIdList(List<string> idList)
        {
            if (idList == null || !idList.Any())
                return null;
            var list = await _roleMongo.GetAsync(x => idList.Contains(x.Id));
            if (list == null || !list.Any()) return null;

            return list.ToList();
        }

        /// <summary>
        /// 分页查询角色
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetListByPage(PageModel<RoleForPageRequest, RoleForListDto> p)
        {
            var r = p.Request;
            MongoModel<Role> mongo = new MongoModel<Role>();
            mongo.Ascending(p.Sort);
            if (r.Name.EnabledStr()) mongo.Where(x => x.Name == r.Name && x.Version == 1.0);
            //mongo.Exclude("Baseinfo.DeliveryAddressList");


            //根据条件查询记录数
            var count = await _roleMongo.CountAsync(mongo.Filter.And(mongo.FilterList));

            //分页查询
            var list = (await _roleMongo.GetByPageAsync(mongo, p.PageIndex, p.PageSize)).ToList();
            if (!list.Any())
                return BaseResponse.GetBaseResponse(BusinessStatusType.NoData, "未查询到数据，请稍后重试");

            var dtoList = list.Select(x => RoleForListDto.MapTo(x)).ToList();



            p.Data = dtoList;
            p.DataCount = count;

            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, p);
        }



        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse> CreatePermission(PermissionForCreateRequest r)
        {
            var permission = new Permission
            {
                Name = r.Name,
                Type = r.Type,
                Content = r.Content
            };

            var flag = await _permissionMongo.AddAsync(permission);
            if (!flag) return BaseResponse.GetBaseResponse(BusinessStatusType.OperateError);
            return BaseResponse.GetBaseResponse(BusinessStatusType.OK);
        }

        /// <summary>
        /// 获取权限详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetPermissionDetail(string id)
        {
            var permission = await _permissionMongo.GetFirstOrDefaultAsync(x => x.Id == id && x.Version == 1.0);
            if (permission == null) return BaseResponse.GetBaseResponse(BusinessStatusType.NoData);

            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, permission);
        }

        /// <summary>
        /// 获取所有权限-树形结构
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetPermissionList(RoleForPageRequest p)
        {
            //查询
            var list = (await _permissionMongo.GetAsync(x => x.Name == p.Name)).ToList();
            if (!list.Any())
                return BaseResponse.GetBaseResponse(BusinessStatusType.NoData, "未查询到数据，请稍后重试");

            return BaseResponse.GetBaseResponse(BusinessStatusType.OK, null, p);
        }

    }
}

using AutoMapper;
using Iris.Models.Model.AgentPart;
using Iris.MongoDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Service.Service
{
    public class AuthService
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




    }
}

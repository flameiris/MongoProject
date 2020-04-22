using Iris.Models.Common;
using Iris.Models.Request.AgentPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iris.Identity.Service
{
    public interface IAgentService
    {
        Task<dynamic> GetAgentByNameAndPwd(AgentForIdCenterRequest r);
    }
}

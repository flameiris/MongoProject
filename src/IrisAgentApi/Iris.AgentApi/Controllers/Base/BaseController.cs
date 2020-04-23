using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Iris.AgentApi.Controllers.Base
{
    [ApiController]
    [Route("agentapi/[controller]/[action]")]
    //[Authorize]
    public class BaseController : ControllerBase
    {
        private readonly ILogger _logger;

        public BaseController()
        {

        }
        public BaseController(
            ILogger<HomeController> logger
            )
        {
            _logger = logger;
        }


    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Iris.AgentApi.Controllers.Base
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public BaseController(
            ILogger<HomeController> logger
            , IMapper mapper
            )
        {
            _logger = logger;
            _mapper = mapper;
        }


    }
}
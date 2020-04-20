using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Iris.UserApi.Controllers.Base
{
    [Route("api/home/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public HomeController(
            ILogger<HomeController> logger
            , IMapper mapper
            )
        {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public string Index()
        {
            return "OK";
        }

        [Authorize]
        [HttpGet]
        public JsonResult Index2(string name = "abc", int age = 18)
        {
            return new JsonResult(from c in HttpContext.User.Claims select new { c.Type, c.Value });
        }
    }
}
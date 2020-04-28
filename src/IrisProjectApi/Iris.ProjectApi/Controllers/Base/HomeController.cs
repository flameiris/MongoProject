using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Iris.ProjectApi.Controllers.Base
{
    [Route("api/home/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(
            ILogger<HomeController> logger
            )
        {
            _logger = logger;
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
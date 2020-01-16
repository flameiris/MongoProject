using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iris.FrameCore.MongoDb;
using Iris.Models.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Iris.Api.Controllers.Base
{
    [Route("api/home/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly IMongoDbManager<User> _userMongo;

        public HomeController(
            ILogger<HomeController> logger,

            IMongoDbManager<User> userMongo
            )
        {
            _logger = logger;

            _userMongo = userMongo;
        }

        public async Task<string> Index()
        {

            _logger.LogInformation("OK");

            await _userMongo.AddAsync(new User
            {
                Username = "FlameIris",
                Password = "123456"
            });

            return "OK";
        }

        public async Task<string> Get()
        {

            var list1 = await _userMongo.GetAsync(x => x.Username == "FlameIris");

            var list2 = await _userMongo.GetAsync(x => x.Username == "FlameIris2");
            return "OK";
        }
    }
}
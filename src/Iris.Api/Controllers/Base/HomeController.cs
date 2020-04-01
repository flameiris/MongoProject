using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iris.FrameCore.MongoDb;
using Iris.Models.Dto;
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

        [HttpGet]
        public string Index2(string name = "abc", int age = 18)
        {
            return "OK";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iris.AgentApi.Controllers.Base;
using Iris.Models.Common;
using Iris.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iris.AgentApi.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(
            IAuthService authService
            )
        {
            _authService = authService;
        }



    }
}
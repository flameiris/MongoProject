using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestApi.IService;

namespace TestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController(

            )
        {

        }

        public string Index(
            [FromServices]IOrderService _orderService,
            [FromServices]IOrderV2Service _orderV2Service,
            [FromServices]IHostApplicationLifetime _hostApplicationLifetime
            //[FromServices]IApplicationBuilder applicationBuilder
            )
        {
            Console.WriteLine($"_orderService hashcode={_orderService.GetHashCode()}");
            //_hostApplicationLifetime.StopApplication();

            _orderV2Service.ShowCode();



            return "ok";
        }




       
    }
}
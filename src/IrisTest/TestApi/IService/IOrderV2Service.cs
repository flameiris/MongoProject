using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.IService
{
    public interface IOrderV2Service
    {
        void ShowCode();
    }

    public class OrderV2Service : IOrderV2Service
    {
        public void ShowCode()
        {
            Console.WriteLine($"OrderV2Servie.ShowCode:{GetHashCode()}");
        }
    }

    public class OrderV3Service : IOrderV2Service
    {
        public MyNameService Name { get; set; }
        public void ShowCode()
        {
            Console.WriteLine($"OrderV3Servie.ShowCode:{GetHashCode()}, OrderV3Servie.Name是否为空:{Name == null}");
        }
    }

    public class MyNameService
    {

    }
}

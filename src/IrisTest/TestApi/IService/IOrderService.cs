using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.IService
{
    public interface IOrderService
    {
    }

    public class OrderService : IOrderService, IDisposable
    {
        public OrderService()
        {
            Console.WriteLine($"OrderService hashcode={this.GetHashCode()} 实例创建");
        }
        public void Dispose()
        {
            Console.WriteLine($"OrderService hashcode={this.GetHashCode()} 实例释放");
        }
    }
}

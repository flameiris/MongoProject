using Iris.AgentApi.Extensions.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Iris.AgentApi.Extensions.Middleware
{
    public static class ExceptionHandlingExtensions
    {
        /// <summary>
        /// 异常中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }

    }
}
using Iris.FrameCore.ApiExtensions.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Iris.FrameCore.ApiExtensions.Middleware
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
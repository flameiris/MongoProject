using Microsoft.AspNetCore.Builder;
using Ocelot.Middleware.Pipeline;

namespace Iris.Ocelot.Middlewares.Responder.Middleware
{
    public static class ResponderMiddlewareExtensions
    {
        public static IOcelotPipelineBuilder UseCustomResponderMiddleware(this IOcelotPipelineBuilder builder)
        {
            return builder.UseMiddleware<ResponderMiddleware>();
        }
    }
}

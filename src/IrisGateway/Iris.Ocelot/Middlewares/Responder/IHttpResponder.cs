using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;

namespace Iris.Ocelot.Middlewares.Responder
{
    public interface IHttpResponder
    {
        Task SetResponseOnHttpContext(HttpContext context, DownstreamResponse response);
        void SetErrorResponseOnContext(HttpContext context, int statusCode);
    }
}

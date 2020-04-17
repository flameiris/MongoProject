using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Iris.UserApi.Extensions.Middlewares
{
    /// <summary>
    /// 异常捕获中间件
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlerMiddleware(RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var body = string.Empty;
            try
            {
                if (context.Request.Method.ToUpper() == "POST")
                {
                    context.Request.EnableRewind();
                    using (var mem = new MemoryStream())
                    {
                        context.Request.Body.CopyTo(mem);
                        mem.Position = 0;
                        using (StreamReader reader = new StreamReader(mem, Encoding.UTF8))
                        {
                            body = reader.ReadToEnd();
                            context.Request.Body.Position = 0;
                        }
                    }
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, body, _logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, string body, ILogger _logger)
        {
            string errorMsg = ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException;
            WriteErrorLog(context, ex, body, _logger);
            PathString path = context.Request.Path;
            Regex regex = new Regex("/api/v.*?/car([^\\w]|$)");
            //if (regex.IsMatch(path.Value))
            //{
            //    var data = RbSysBaseResponse<object>.GetResponse(BusinessStatusType.SysError);
            //    var result = JsonConvert.SerializeObject(data);
            //    context.Response.ContentType = "application/json;charset=utf-8";
            //    return context.Response.WriteAsync(result);
            //}
            //else
            //{
            //    var data = BaseResponse.GetBaseResponse(BusinessStatusType.SysError);
            //    var result = JsonConvert.SerializeObject(data);
            //    context.Response.ContentType = "application/json;charset=utf-8";
            //    return context.Response.WriteAsync(result);
            //}


            return context.Response.WriteAsync("");

        }

        private static void WriteErrorLog(HttpContext httpContent, Exception ex, string body, ILogger _logger)
        {
            StringBuilder logBuilder = new StringBuilder();
            logBuilder.Append($"{Environment.NewLine}请求API地址:");
            logBuilder.Append(httpContent.Request.Host.ToString() + httpContent.Request.Path.ToString() + httpContent.Request.QueryString.Value + Environment.NewLine);

            var sign = httpContent.Request.Headers["Sign"];
            var strTimestamp = httpContent.Request.Headers["Timestamp"];
            var platform = httpContent.Request.Headers["Platform"];

            logBuilder.Append($"请求头:");
            logBuilder.Append($"Sign={sign}");
            logBuilder.Append($"Timestamp={strTimestamp}");
            logBuilder.Append($"Platform={platform}{Environment.NewLine}");
            if (!string.IsNullOrWhiteSpace(body) && httpContent.Request.ContentType.ToLower().Contains("json"))
            {
                logBuilder.Append($"请求Body:");
                //logBuilder.Append(new JsonObject(body).Json + Environment.NewLine);
            }
            _logger.LogError(ex, logBuilder.ToString());
        }
    }
}
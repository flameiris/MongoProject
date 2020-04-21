using Iris.Models.Common;
using Iris.Models.Enums;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace Iris.UserApi.Filters
{
    public class RequestAuthorizeAttribute : ActionFilterAttribute
    {
        private bool IsGlobal { get; set; } = false;
        private readonly ILogger _logger;

        public RequestAuthorizeAttribute(
            bool isGlobal,
            ILogger<RequestAuthorizeAttribute> logger
            )
        {
            IsGlobal = isGlobal;
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            StringBuilder logBuilder = new StringBuilder();
            logBuilder.Append($"{Environment.NewLine}URL:{Environment.NewLine}");
            logBuilder.Append(context.HttpContext.Request.Host + context.HttpContext.Request.Path + context.HttpContext.Request.QueryString.Value + Environment.NewLine);
            var method = context.HttpContext.Request.Method.ToUpper();
            logBuilder.Append($"Method={method}{Environment.NewLine}");

            var body = string.Empty;
            if (method == "POST" || method == "PUT")
            {
                var contentType = context.HttpContext.Request.ContentType?.ToUpper();
                logBuilder.Append($"ContentType={contentType}{Environment.NewLine}");

                var contentLength = context.HttpContext.Request.ContentLength;
                if (contentLength.HasValue && contentLength > 0)
                {
                    if (contentType.Contains("JSON"))
                    {
                        if (contentLength <= int.MaxValue)
                        {
                            context.HttpContext.Request.EnableRewind();
                            using (var mem = new MemoryStream())
                            {
                                context.HttpContext.Request.Body.Position = 0;
                                context.HttpContext.Request.Body.CopyTo(mem);
                                mem.Position = 0;
                                using (StreamReader reader = new StreamReader(mem, Encoding.UTF8))
                                {
                                    body = reader.ReadToEnd();
                                }
                            }
                        }
                        else
                        {
                            var data = BaseResponse.GetBaseResponse(BusinessStatusType.ParameterUnDesirable, "请求长度超过限制！");
                            context.Result = new JsonResult(data)
                            {
                                StatusCode = (int)BusinessStatusType.ParameterUnDesirable
                            };
                            return;
                        }
                    }
                }
            }

            logBuilder.Append($"Body:{Environment.NewLine}");
            logBuilder.Append(body + Environment.NewLine);
            _logger.LogInformation(logBuilder.ToString());




            base.OnActionExecuting(context);
        }
    }
}

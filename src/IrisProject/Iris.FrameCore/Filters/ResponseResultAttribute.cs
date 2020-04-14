using Iris.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Iris.FrameCore.Filters
{
    public class ResponseResultAttribute : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result != null)
            {
                if (context.Result is ObjectResult objResult)
                {
                    if (objResult.Value is BaseResponse response)
                        if (response != null)
                        {
                            if (response.Code < 200 || response.Code > 299)
                            {
                                context.HttpContext.Response.StatusCode = response.Code;
                            }
                        }
                }
            }
        }
    }
}
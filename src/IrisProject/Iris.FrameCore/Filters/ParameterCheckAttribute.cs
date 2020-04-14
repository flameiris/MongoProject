using Iris.Models.Common;
using Iris.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Iris.FrameCore.Filters
{
    public class ParameterCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.ActionArguments.Where(o => o.Value != null).Any())
            {
                if (!context.ModelState.IsValid)
                {
                    var listErrMsg = new List<string>();
                    foreach (var value in context.ModelState.Values)
                    {
                        if (value.Errors.AsEnumerable().Any())
                        {
                            var errMsg = value.Errors.FirstOrDefault().ErrorMessage;
                            if (string.IsNullOrEmpty(errMsg))
                            {
                                errMsg = value.Errors.FirstOrDefault().Exception.Message;
                            }
                            listErrMsg.Add(errMsg);
                        }
                    }

                    var errorMsg = string.Join(" ", listErrMsg);
                    var data = BaseResponse.GetBaseResponse(BusinessStatusType.ParameterUnDesirable, errorMsg);
                    context.Result = new JsonResult(data)
                    {
                        StatusCode = (int)BusinessStatusType.ParameterUnDesirable
                    };
                    return;
                }
            }
        }
    }
}

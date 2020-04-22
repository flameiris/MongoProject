using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Iris.Identity.CustomFilter
{
    public class CustomResponseFitler : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {



            base.OnActionExecuted(context);
        }
    }
}

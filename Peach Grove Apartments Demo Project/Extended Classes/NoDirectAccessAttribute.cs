using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peach_Grove_Apartments_Demo_Project.Extended_Classes
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //public class NoDirectAccessAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        if (filterContext.HttpContext.Request == null ||
    // filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
    //        {
    //            filterContext.Result = new RedirectToRouteResult(new
    //                                      RouteValueDictionary(new { controller = "Home", action = "Logout", area = "Main" }));
    //        }
    //    }
    //}
}

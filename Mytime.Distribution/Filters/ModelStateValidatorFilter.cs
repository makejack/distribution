using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mytime.Distribution.Models;
using Mytime.Distribution.Models.V1.Response;

namespace Mytime.Distribution.Filters
{
    /// <summary>
    /// 模型验证过滤
    /// </summary>
    public class ModelStateValidatorFilter : IActionFilter
    {
        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var modelStateDictonary = context.ModelState;

                List<ModelValidatorFailedResponse> errorResults = new List<ModelValidatorFailedResponse>();
                foreach (var key in modelStateDictonary.Keys)
                {
                    errorResults.Add(new ModelValidatorFailedResponse(key, modelStateDictonary[key].Errors.Select(e => e.ErrorMessage).FirstOrDefault()));
                }

                context.Result = new JsonResult(Result.Fail(ResultCodes.RequestParamError, errorResults));
            }
        }
    }
}
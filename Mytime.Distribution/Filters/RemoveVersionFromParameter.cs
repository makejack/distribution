using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mytime.Distribution.Filters
{
    /// <summary>
    /// 移除版本号
    /// </summary>
    public class RemoveVersionFromParameter : IOperationFilter
    {
        /// <summary>
        /// 申请
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            operation.Parameters.Remove(versionParameter);
        }
    }
}
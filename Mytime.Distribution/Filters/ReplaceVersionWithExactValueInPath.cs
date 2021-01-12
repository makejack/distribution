using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mytime.Distribution.Filters
{
    /// <summary>
    /// 替换版本号
    /// </summary>
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        /// <summary>
        /// 申请
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("v{version}", $"v{swaggerDoc.Info.Version}"), path.Value);
            }
            swaggerDoc.Paths = paths;
        }
    }
}
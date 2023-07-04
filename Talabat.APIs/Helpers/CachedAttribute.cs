using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Drawing.Printing;
using System.Text;
using System.Xml.Linq;
using Talabat.Core.IService;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds )
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>(); // RequestServices => access to Contanier

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result= contentResult;
                return;
            }

            var executedEndpointContext = await next(); // will Execute the EndPoint

            if  (executedEndpointContext.Result is OkObjectResult okObjectResult) // Result stored in okObjectResult
            {
                await cacheService.CacheResponseAsync(cacheKey ,okObjectResult.Value,TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {   
            // {{Url}}/api/Products?pageIndex=1&pageSize=5&sort=name

            var keyBuilder =new StringBuilder();

            keyBuilder.Append(request.Path); //  /api/Product

           // pageIndex = 1
           // pageSize = 5
           // sort = name

            foreach (var (key,value) in request.Query.OrderBy(O => O.Key)) // because make Sort
            {
                keyBuilder.Append($"|{key}-{value}");
                // api / Products| pageIndex - 1
                // api / Products| pageIndex - 1| pageSize - 5
                // api / Products| pageIndex - 1| pageSize - 5| sort - name
            }

            return keyBuilder.ToString();

        }
    }
}

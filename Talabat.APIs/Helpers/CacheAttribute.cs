using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Services.CashingService;

namespace Talabat.APIs.Helpers
{
    public class CacheAttribute:Attribute
    {
        private readonly int _timeToLiveInSeconds;

        public CacheAttribute(int TimeToLiveInSeconds)
        {
            _timeToLiveInSeconds = TimeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context,ActionExecutionDelegate _next)
        {
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var CachKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var CachedResponse =await CacheService.GetCacheResponse(CachKey);
            if (! string.IsNullOrEmpty(CachedResponse))
            {
                var ContentResult = new ContentResult()
                {
                    Content = CachedResponse,
                    ContentType = "Application/json",
                    StatusCode = 200
                };
                return;
            }
            var ExecutedContext= await _next();
            if (ExecutedContext.Result is OkObjectResult Response)
            {
                await CacheService.SetCacheResponse(CachKey, Response.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }

        }
        public string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var CacheKey = new StringBuilder();
            CacheKey.Append($"{request.Path}");
            foreach (var (Key,Value) in request.Query.OrderBy(x=>x.Key))
            {
                CacheKey.Append($"| {Key}-{Value}");
            }
            return CacheKey.ToString();
        }
    }
}

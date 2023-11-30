using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Talabat.Services.CashingService
{
    public class CashService : ICacheService
    {
        private readonly IDatabase _database;
        public CashService(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();
        }
        public async Task SetCacheResponse(string CachKey, object Response, TimeSpan TimeToLive)
        {
            if (Response is null)
                return;
            var Options = new JsonSerializerOptions() { PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
            var SerializedResponse = JsonSerializer.Serialize(Response, Options);
            await _database.StringSetAsync(CachKey,SerializedResponse,TimeToLive);
        }
        public async Task<string?> GetCacheResponse(string CachKey)
        {
          var CashedResponse= await _database.StringGetAsync(CachKey);
            if (CashedResponse.IsNullOrEmpty)
                return null;
            return CashedResponse;

        }

    }
}

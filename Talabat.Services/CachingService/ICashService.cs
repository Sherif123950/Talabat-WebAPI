using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Services.CashingService
{
    public interface ICacheService
    {
        Task SetCacheResponse(string CachKey,object Response,TimeSpan TimeToLive);
        Task<string?> GetCacheResponse(string CachKey);
    }
}

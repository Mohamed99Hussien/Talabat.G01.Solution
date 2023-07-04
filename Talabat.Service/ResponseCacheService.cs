using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.IService;

namespace Talabat.Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis) 
        {
            _database = redis.GetDatabase(); 
        }
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response == null) return;

            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; //Because keep small letters
            var serializedResopnse = JsonSerializer.Serialize(response , options);

            await _database.StringSetAsync(cacheKey,serializedResopnse,timeToLive);
            
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cacgedResponse =await _database.StringGetAsync(cacheKey);
            if (cacgedResponse.IsNullOrEmpty) return null;
            return cacgedResponse;
        }
    }
}

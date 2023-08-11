using TalabatBLL.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace TalabatDAL.Services
{
    public class ResponseCasheService : IResponseCasheService
    {
        private readonly IDatabase _database;
        private readonly ILogger<ResponseCasheService> _logger;

        public ResponseCasheService(IConnectionMultiplexer redis, ILogger<ResponseCasheService> logger)
        {
            _database = redis.GetDatabase();
            _logger = logger;
        }
        public async Task CasheResponseAsync(string casheKey, object response, TimeSpan timeToLive)
        {
            if (response is null) return;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy= JsonNamingPolicy.CamelCase,
            };

            var serializedResponse = JsonSerializer.Serialize(response, options);
            await _database.StringSetAsync(casheKey, serializedResponse, timeToLive);

        }

        public async Task<string> GetCasheResponse(string casheKey)
        {
            var cashedResponse = await _database.StringGetAsync(casheKey);
            _logger.LogInformation("==========Cashe Response=========");

            if(cashedResponse.IsNullOrEmpty) return null;
            return cashedResponse.ToString();
        }
    }
}

using Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Net;

namespace Producer
{
    public class CreateTransaction(ILoggerFactory loggerFactory, IMongoDatabase mongoDatabase)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<CreateTransaction>();

        [Function("CreateTransaction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            
            var transactions = mongoDatabase.GetCollection<Transaction>("operations");
            
            await transactions.InsertOneAsync(new Transaction { Amount = (decimal)Random.Shared.NextDouble() });

            await response.WriteStringAsync("Welcome to Azure Functions!");
            return response;
        }
    }
}

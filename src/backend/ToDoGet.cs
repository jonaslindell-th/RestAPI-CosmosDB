using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDB_RestAPI
{
    public static class ToDoGet
    {
        [FunctionName("ToDoGet")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, [CosmosDB(databaseName: "my-database", collectionName: "my-container",
            ConnectionStringSetting = "CosmosDbConnectionString")] DocumentClient client,
            ILogger log)
        {
            var searchterm = req.Query["searchterm"];
            if (string.IsNullOrWhiteSpace(searchterm))
            {
                return (ActionResult)new NotFoundResult();
            }

            Uri collectionUri = UriFactory.CreateDocumentCollectionUri("my-database", "my-container");

            log.LogInformation($"Searching for: {searchterm}");

            IDocumentQuery<ToDoModel> query = client.CreateDocumentQuery<ToDoModel>(collectionUri, new FeedOptions { EnableCrossPartitionQuery = true })
                .Where(p => p.Title.Contains(searchterm))
                .AsDocumentQuery();

            while (query.HasMoreResults)
            {
                foreach (ToDoModel result in await query.ExecuteNextAsync())
                {
                    log.LogInformation(result.Description);
                }
            }
            return new OkObjectResult(query);
        }
    }
}

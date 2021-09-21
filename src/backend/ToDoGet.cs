using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CosmosDB_RestAPI
{
    public static class ToDoGet
    {
        [FunctionName("ToDoGet")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todo/{title}")] HttpRequest req, [CosmosDB(databaseName: "my-database", collectionName: "my-container",
            ConnectionStringSetting = "CosmosDbConnectionString",
            SqlQuery ="SELECT * FROM c WHERE c.title={title}"
            )]IEnumerable<ToDoModel> todos,
            ILogger log,
            string title)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (todos == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(todos);
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CosmosDB_RestAPI
{
    public static class RestAPI
    {
        [FunctionName("RestAPI")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Add-New-ToDo")] HttpRequest req, [CosmosDB(databaseName: "my-database", collectionName: "my-container",
            ConnectionStringSetting = "CosmosDbConnectionString"
            )]IAsyncCollector<object> todo,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                 string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var input = JsonConvert.DeserializeObject<ToDoModel>(requestBody);

                var newTodo = new ToDoModel 
                {
                    Title = input.Title,
                    Description = input.Description
                };

                await todo.AddAsync(newTodo);

                return new OkObjectResult(newTodo);
            }
            catch (Exception ex)
            {
                log.LogError($"Couldn't insert item. Exception thrown: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

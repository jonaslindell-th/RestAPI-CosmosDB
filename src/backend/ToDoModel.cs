using System;
using Newtonsoft.Json;

namespace CosmosDB_RestAPI
{
    public class ToDoModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Date { get; set; }
    }
}
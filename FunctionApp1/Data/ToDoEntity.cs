using ClassLibrary1;
using Newtonsoft.Json;
using System;

namespace FunctionApp1.Data
{
    public class ToDoEntity : Entity<Guid>
    {

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public ToDoEntity()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

using Newtonsoft.Json;
using System;

namespace FunctionApp1.Data
{
    public abstract class Entity<TKey>
    {
        [JsonProperty("id")]
        public TKey Id { get; set; }


        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        protected Entity()
        {
            this.CreatedOn = DateTime.UtcNow;
        }
    }
}

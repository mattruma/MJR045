namespace FunctionApp1.Data
{
    public class EntityDataStoreOptions
    {
        public string ConnectionString { get; set; }

        public EntityDataStoreOptions()
        {
        }

        public EntityDataStoreOptions(
            string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}

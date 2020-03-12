using ClassLibrary1.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FunctionApp1.Data
{
    public class ToDoEntityDataStore : EntityDataStore<Guid, ToDoEntity>, IToDoEntityDataStore
    {
        public ToDoEntityDataStore(
            IAzureTokenProvider azureTokenProvider,
            EntityDataStoreOptions entityDataStoreOptions) : base(azureTokenProvider, entityDataStoreOptions)
        {
        }

        [SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "<Pending>")]
        public override async Task AddAsync(
            ToDoEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id.ToString()))
            {
                throw new ArgumentNullException(nameof(entity.Id));
            }

            var query =
                "INSERT INTO ToDos (Id, Object, CreatedOn, ToDoId, Status, Description) VALUES (@Id, @Object, @CreatedOn, @ToDoId, @Status, @Description)";

            using (var cn = new SqlConnection(_entityDataStoreOptions.ConnectionString))
            {
                cn.AccessToken =
                    await _azureTokenProvider.GetTokenAsync();

                using var cmd = new SqlCommand(query, cn);
                {
                    cn.Open();

                    cmd.Parameters.AddWithValue("@Id", entity.Id);
                    cmd.Parameters.AddWithValue("@Status", entity.Status);
                    cmd.Parameters.AddWithValue("@Description", entity.Description);
                    cmd.Parameters.AddWithValue("@CreatedOn", entity.CreatedOn);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public override Task DeleteByIdAsync(
            Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<ToDoEntity> GetByIdAsync(
            Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<ToDoEntity>> ListAsync(
            int page = 1,
            int pageSize = 100)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync(
            ToDoEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

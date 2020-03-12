using ClassLibrary1.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionApp1.Data
{
    public abstract class EntityDataStore<TKey, TEntity> where TEntity : Entity<TKey>, new()
    {
        protected readonly IAzureTokenProvider _azureTokenProvider;
        protected readonly EntityDataStoreOptions _entityDataStoreOptions;

        protected EntityDataStore(
            IAzureTokenProvider azureTokenProvider,
            EntityDataStoreOptions entityDataStoreOptions)
        {
            if (azureTokenProvider == null)
            {
                throw new ArgumentNullException(nameof(azureTokenProvider));
            }

            _azureTokenProvider =
                azureTokenProvider;

            if (entityDataStoreOptions == null)
            {
                throw new ArgumentNullException(nameof(entityDataStoreOptions));
            }

            if (entityDataStoreOptions.ConnectionString == null)
            {
                throw new ArgumentNullException(nameof(entityDataStoreOptions.ConnectionString));
            }

            _entityDataStoreOptions =
                entityDataStoreOptions;
        }

        public abstract Task AddAsync(
            TEntity entity);

        public abstract Task DeleteByIdAsync(
            TKey id);

        public abstract Task<TEntity> GetByIdAsync(
            TKey id);

        public abstract Task<IEnumerable<TEntity>> ListAsync(
            int page = 1,
            int pageSize = 100);

        public abstract Task UpdateAsync(
            TEntity entity);
    }
}

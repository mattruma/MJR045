using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionApp1.Data
{
    public interface IToDoEntityDataStore
    {
        public Task AddAsync(
            ToDoEntity entity);

        public Task DeleteByIdAsync(
            Guid id);

        public Task<ToDoEntity> GetByIdAsync(
            Guid id);

        public Task UpdateAsync(
            ToDoEntity entity);

        Task<IEnumerable<ToDoEntity>> ListAsync(
            int page,
            int pageSize);
    }
}

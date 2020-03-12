using FunctionApp1.Data;
using System;

namespace FunctionApp1
{
    public class ToDo
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }

        public ToDo()
        {
        }

        public ToDo(
            ToDoEntity toDoEntity)
        {
            this.Id = toDoEntity.Id;
            this.Status = toDoEntity.Status;
            this.Description = toDoEntity.Description;
            this.CreatedOn = toDoEntity.CreatedOn;
        }
    }
}

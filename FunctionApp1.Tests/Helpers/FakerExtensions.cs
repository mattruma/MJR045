using Bogus;
using FunctionApp1.Data;
using System;

namespace FunctionApp1.Tests.Helpers
{
    public static class FakerExtensions
    {
        public static ToDoEntity GenerateToDoEntity(
            this Faker faker)
        {
            var toDoEntity =
                new ToDoEntity
                {
                    Id = Guid.NewGuid(),
                    Status = faker.Random.ArrayElement(new[] { "Pending", "In Progress", "Completed", "Canceled" }),
                    Description = faker.Lorem.Paragraph(1),
                    CreatedOn = faker.Date.Recent(10)
                };

            return toDoEntity;
        }
    }
}

using ClassLibrary1.Helpers;
using FluentAssertions;
using FunctionApp1.Data;
using FunctionApp1.Tests.Helpers;
using Moq;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;

namespace FunctionApp1.Tests
{
    public class ToDoEntityDataStoreTests : BaseTests
    {
        [Fact]
        public async Task When_AddAsync_ThenToDoAdded()
        {
            // Arrange

            var entityDataStoreOptions =
                new EntityDataStoreOptions
                {
                    ConnectionString = _configuration["AzureSqlOptions:ConnectionString"]
                };

            var azureTokenProviderOptions =
                new AzureTokenProviderOptions
                {
                    Authority = _configuration["TokenProviderOptions:Authority"],
                    ClientId = _configuration["TokenProviderOptions:ClientId"],
                    ClientSecret = _configuration["TokenProviderOptions:ClientSecret"],
                    ResourceId = "https://database.windows.net/",
                    TenantId = _configuration["TokenProviderOptions:TenantId"]
                };

            var azureTokenProvider =
                new AzureTokenProvider(
                    azureTokenProviderOptions);

            var toDoEntityDataStore =
                new ToDoEntityDataStore(
                    azureTokenProvider,
                    entityDataStoreOptions);

            var toDoEntity =
                _faker.GenerateToDoEntity();

            // Action

            await toDoEntityDataStore.AddAsync(
                toDoEntity);

            // Assert

            var query =
                "SELECT * FROM todos WHERE Id = @Id";

            using var cn = new SqlConnection(_configuration["AzureSqlOptions:ConnectionString"]);
            using var cmd = new SqlCommand(query, cn);

            cn.AccessToken =
                await azureTokenProvider.GetTokenAsync();

            cn.Open();

            cmd.Parameters.AddWithValue("@Id", toDoEntity.Id);

            var reader = await cmd.ExecuteReaderAsync();

            ToDoEntity toDoEntityFetched = null;

            while (reader.Read())
            {
                toDoEntityFetched = new ToDoEntity
                {
                    Id = reader.GetGuid(0),
                    Status = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedOn = reader.GetDateTime(3)
                };
            }

            toDoEntityFetched.Should().NotBeNull();
            toDoEntityFetched.Id.Should().Be(toDoEntity.Id);
            toDoEntityFetched.Status.Should().Be(toDoEntity.Status);
            toDoEntityFetched.Description.Should().Be(toDoEntity.Description);
            toDoEntityFetched.CreatedOn.Should().BeCloseTo(toDoEntity.CreatedOn);
        }

        [Fact]
        public async Task Given_BadToken_When_AddAsync_ThrowException()
        {
            // Arrange

            var entityDataStoreOptions =
                new EntityDataStoreOptions
                {
                    ConnectionString = _configuration["AzureSqlOptions:ConnectionString"]
                };

            var azureTokenProvider =
                new Mock<IAzureTokenProvider>();

            azureTokenProvider.Setup(x => x.GetTokenAsync()).ReturnsAsync("BADTOKEN");

            var toDoEntityDataStore =
                new ToDoEntityDataStore(
                    azureTokenProvider.Object,
                    entityDataStoreOptions);

            var toDoEntity =
                _faker.GenerateToDoEntity();

            // Action

            Func<Task> action = async () => await toDoEntityDataStore.AddAsync(
                toDoEntity);

            // Assert

            action.Should().Throw<SqlException>().WithMessage("Login failed for user 'NT AUTHORITY\\ANONYMOUS LOGON'.");
        }
    }
}
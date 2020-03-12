using Bogus;
using FunctionApp1.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace FunctionApp1.Tests
{
    public abstract class BaseTests : IAsyncLifetime
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger _logger = LoggerHelper.CreateLogger(LoggerTypes.List);
        protected readonly Faker _faker;

        protected BaseTests()
        {
            // NOTE: Make sure to set these files to copy to output directory

            _configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                 .AddJsonFile("appsettings.Development.json")
                 .Build();

            _faker = new Faker();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}

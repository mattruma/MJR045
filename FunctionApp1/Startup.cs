using ClassLibrary1;
using ClassLibrary1.Helpers;
using FunctionApp1.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]
namespace FunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(
            IFunctionsHostBuilder builder)
        {
            var services =
                builder.Services;

            var entityDataStoreOptions =
                new EntityDataStoreOptions
                {
                    ConnectionString = Environment.GetEnvironmentVariable("AzureSqlOptions:ConnectionString")
                };

            services.AddSingleton(entityDataStoreOptions);

            var azureTokenProviderOptions =
                new AzureTokenProviderOptions
                {
                    Authority = Environment.GetEnvironmentVariable("TokenProviderOptions:Authority"),
                    ClientId = Environment.GetEnvironmentVariable("TokenProviderOptions:ClientId"),
                    ClientSecret = Environment.GetEnvironmentVariable("TokenProviderOptions:ClientSecret"),
                    ResourceId = "https://database.windows.net/",
                    TenantId = Environment.GetEnvironmentVariable("TokenProviderOptions:TenantId")
                };

            services.AddSingleton(azureTokenProviderOptions);

            services.AddTransient<IAzureTokenProvider, AzureTokenProvider>();
            services.AddTransient<IToDoEntityDataStore, ToDoEntityDataStore>();
        }
    }
}

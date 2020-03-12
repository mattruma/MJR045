using ClassLibrary1.Helpers;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ClassLibrary1.Tests
{
    public class AzureTokenProviderTests : BaseTests
    {
        [Fact]
        public async Task When_RefreshTokenAsync_Then_ReturnToken()
        {
            // Arrange

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

            // Action

            var accessToken =
                await azureTokenProvider.RefreshTokenAsync();

            // Assert

            accessToken.Should().NotBeNull();
        }

        [Fact]
        public async Task Given_TokenIsInCache_When_GetTokenAsync_Then_ReturnToken()
        {
            // Arrange

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

            var accessToken =
                await azureTokenProvider.RefreshTokenAsync();

            // Action

            var cachedAccessToken =
                await azureTokenProvider.GetTokenAsync();

            // Assert

            cachedAccessToken.Should().NotBeNull();
            cachedAccessToken.Should().Be(accessToken);
        }
    }
}

using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Polly;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClassLibrary1.Helpers
{
    public class AzureTokenProvider : IAzureTokenProvider
    {
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        private const int DEFAULT_RETRY_COUNT = 3;

        // AADSTS7000215 : Invalid client secret is provided.
        // AADSTS700016  : Application with identifier 'CLIENT_ID' was not found in the directory 'TENANT_ID'. This can happen if the application has not been installed by the administrator of the tenant or consented to by any user in the tenant. You may have sent your authentication request to the wrong tenant.

        private static string _accessToken;
        private readonly AzureTokenProviderOptions _azureTokenProviderOptions;

        public IAzureTokenValidator AzureTokenValidator { get; set; }

        public AzureTokenProvider(
            AzureTokenProviderOptions azureTokenProviderOptions)
        {
            _azureTokenProviderOptions = azureTokenProviderOptions;

            if (!_azureTokenProviderOptions.HasResourceId)
            {
                throw new ArgumentNullException(nameof(_azureTokenProviderOptions.ResourceId), $"{nameof(_azureTokenProviderOptions.ResourceId)} is required.");
            }

            if (_azureTokenProviderOptions.HasRetryCount
                && _azureTokenProviderOptions.RetryCount.Value < 1)
            {
                throw new ArgumentNullException(nameof(_azureTokenProviderOptions.RetryCount), $"{nameof(_azureTokenProviderOptions.RetryCount)} must be greater than zero, when provided.");
            }

            this.AzureTokenValidator =
                new AzureTokenValidator();
        }

        public async Task<string> GetTokenAsync()
        {
            if (!string.IsNullOrWhiteSpace(_accessToken)
                && this.AzureTokenValidator != null
                && this.AzureTokenValidator.ValidateToken(_accessToken))
            {
                return _accessToken;
            }

            return await this.RefreshTokenAsync();
        }

        public async Task<string> RefreshTokenAsync()
        {
            // https://github.com/App-vNext/Polly
            // https://github.com/App-vNext/Polly#handing-return-values-and-policytresult
            // https://github.com/App-vNext/Polly/wiki/Retry#exponential-backoff

            await Policy
                // .Handle<Exception>() // NOTE: This will retry on every Exception
                .Handle<HttpRequestException>() // NOTE: Think it is the HttpRequestException that gets thrown if for some reason the token endpoint cannot be reached
                .WaitAndRetryAsync(
                    _azureTokenProviderOptions.RetryCount.GetValueOrDefault(DEFAULT_RETRY_COUNT),
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, retryCount) => Trace.WriteLine($"Retry attempt {retryCount}.")
                ).ExecuteAsync(() => this.AcquireTokenAsync());

            return _accessToken;
        }

        internal async Task<string> AcquireTokenAsync()
        {
            if (_azureTokenProviderOptions.HasAuthority
                    && _azureTokenProviderOptions.HasClientId
                    && _azureTokenProviderOptions.HasClientSecret)
            {
                var authenticationContext =
                    new AuthenticationContext(
                        _azureTokenProviderOptions.Authority);

                var clientCredentials =
                    new ClientCredential(
                        _azureTokenProviderOptions.ClientId,
                        _azureTokenProviderOptions.ClientSecret);

                var authenticationResult =
                    await authenticationContext.AcquireTokenAsync(
                        _azureTokenProviderOptions.ResourceId,
                        clientCredentials);

                _accessToken =
                    authenticationResult.AccessToken;
            }
            else
            {
                var azureTokenProvider =
                    new AzureServiceTokenProvider();

                if (string.IsNullOrWhiteSpace(_azureTokenProviderOptions.TenantId))
                {
                    _accessToken =
                        await azureTokenProvider.GetAccessTokenAsync(_azureTokenProviderOptions.ResourceId);
                }
                else
                {
                    _accessToken =
                        await azureTokenProvider.GetAccessTokenAsync(_azureTokenProviderOptions.ResourceId, _azureTokenProviderOptions.TenantId);
                }
            }

            return _accessToken;
        }
    }
}

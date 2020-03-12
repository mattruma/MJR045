namespace ClassLibrary1.Helpers
{
    public class AzureTokenProviderOptions
    {
        public string Authority { get; set; }

        internal bool HasAuthority => !string.IsNullOrWhiteSpace(this.Authority);

        public string ClientId { get; set; }

        internal bool HasClientId => !string.IsNullOrWhiteSpace(this.ClientId);

        public string ClientSecret { get; set; }

        internal bool HasClientSecret => !string.IsNullOrWhiteSpace(this.ClientSecret);

        public string ResourceId { get; set; }

        internal bool HasResourceId => !string.IsNullOrWhiteSpace(this.ResourceId);

        public string TenantId { get; set; }

        internal bool HasTenantId => !string.IsNullOrWhiteSpace(this.TenantId);

        public int? RetryCount { get; set; }

        internal bool HasRetryCount => this.RetryCount.HasValue;
    }
}

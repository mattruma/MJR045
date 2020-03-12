using System.Threading.Tasks;

namespace ClassLibrary1.Helpers
{
    public interface IAzureTokenProvider
    {
        Task<string> GetTokenAsync();

        Task<string> RefreshTokenAsync();
    }
}

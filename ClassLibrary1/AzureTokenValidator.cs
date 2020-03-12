using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace ClassLibrary1
{
    public class AzureTokenValidator : IAzureTokenValidator
    {
        public bool ValidateToken(
            string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);

            var claim =
                jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "exp");

            if (claim == null)
            {
                throw new ArgumentOutOfRangeException("exp", "Token is missing 'exp' claim.");
            }

            if (!long.TryParse(claim.Value, out var epoch))
            {
                throw new ArgumentOutOfRangeException("exp", $"The value of '{claim.Value}' in the 'exp' claim in the token is not a valid epoch time..");
            }

            var expiresOn =
                this.ConvertEpochToDateTime(epoch);

            // expires On | current Time  | adjusted Expires On | token          | return
            // -----------------------------------------------------------------------------
            // 3:30 PM    |  2:30 PM      | 3:20 PM             | good token     | true
            // 3:30 PM    |  3:25 PM      | 3:20 PM             | expired token  | false
            // 3:30 PM    |  3:20 PM      | 3:20 PM             | expired token  | false
            // 3:30 PM    |  3:19 PM      | 3:20 PM             | good token     | true

            if (DateTime.UtcNow < expiresOn.AddMinutes(-10))
            {
                return true;
            }

            return false;
        }

        private DateTime ConvertEpochToDateTime(
            long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}

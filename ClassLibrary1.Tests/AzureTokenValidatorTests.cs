using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace ClassLibrary1.Tests
{
    public class AzureTokenValidatorTests : BaseTests
    {
        // https://martinfowler.com/bliki/GivenWhenThen.html

        [Fact]
        public void Given_TokenIsExpired_When_ValidateToken_Then_ReturnFalse()
        {
            // Arrange

            var azureTokenValidator =
                new AzureTokenValidator();

            var claims = new List<Claim>();

            var epochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            epochDateTime = epochDateTime.AddDays(-1);

            var epochTime = Convert.ToInt64((DateTime.UtcNow - epochDateTime).TotalSeconds);

            claims.Add(new Claim("exp", epochTime.ToString()));

            var jwtSecurityToken =
                new JwtSecurityToken(claims: claims);

            var jwtSecurityTokenHandler =
                new JwtSecurityTokenHandler();

            var token =
                jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            // Action

            var accessToken =
                azureTokenValidator.ValidateToken(token);

            // Assert

            accessToken.Should().BeFalse();
        }

        [Fact]
        public void Given_TokenIsNotExpired_When_ValidateToken_Then_ReturnFalse()
        {
            // Arrange

            var azureTokenValidator =
                new AzureTokenValidator();

            var claims = 
                new List<Claim>();

            var epochDateTime = 
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            epochDateTime = 
                epochDateTime.AddDays(1);

            var epochTime = 
                Convert.ToInt64((DateTime.UtcNow - epochDateTime).TotalSeconds);

            claims.Add(new Claim("exp", epochTime.ToString()));

            var jwtSecurityToken =
                new JwtSecurityToken(claims: claims);

            var jwtSecurityTokenHandler =
                new JwtSecurityTokenHandler();

            var token =
                jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            // Action

            var accessToken =
                azureTokenValidator.ValidateToken(token);

            // Assert

            accessToken.Should().BeTrue();
        }
    }
}

using LetsDoIt.Moody.Application.Security;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace LetsDoIt.Moody.Application.UnitTests.Security
{
    using Constants;
    using Options;

    public class SecurityServiceTests
    {
        private const int TokenExpirationMinutes = 1440;
        private readonly ISecurityService _testing;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public SecurityServiceTests()
        {
            var mockJwtOptions = new Mock<IOptions<JwtOptions>>();

            var jwtOptions = new JwtOptions
            {
                Audience = "default.audience",
                Issuer = "default.issuer",
                SecretKey = "2hN70OoacUi5SDU0rNuIXg==",
                TokenExpirationMinutes = TokenExpirationMinutes
            };

            mockJwtOptions.Setup(x => x.Value).Returns(jwtOptions);

            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _testing = new SecurityService(mockJwtOptions.Object);
        }

        [Fact]
        public void GenerateJWtToken_ShoudlGenerateJwtTokenBasedOnGivenParameters()
        {
            var id = "good.id";
            var fullName = "good.fullName";

            var actual = _testing.GenerateJwtToken(id, fullName, UserTypeConstants.Admin);
            var jwtToken = _jwtSecurityTokenHandler.ReadJwtToken(actual.Token);

            var claimNameIdentifier = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var claimName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var claimRole = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            Assert.Equal(id, claimNameIdentifier.Value);
            Assert.Equal(fullName, claimName.Value);
            Assert.Equal(UserTypeConstants.Admin, claimRole.Value);
        }

        [Fact]
        public void GenerateJwtToken_GeneratedTokenShouldHaveGivenExpirationDate()
        {
            var id = "good.id";
            var fullName = "good.fullName";

            var actual = _testing.GenerateJwtToken(id, fullName, UserTypeConstants.Admin);
            var jwtToken = _jwtSecurityTokenHandler.ReadJwtToken(actual.Token);

            Assert.InRange(jwtToken.ValidTo, DateTime.UtcNow.AddMinutes(TokenExpirationMinutes - 10), DateTime.UtcNow.AddMinutes(TokenExpirationMinutes + 10));
        }
    }
}

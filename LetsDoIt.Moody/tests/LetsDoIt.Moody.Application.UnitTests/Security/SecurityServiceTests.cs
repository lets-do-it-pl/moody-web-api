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
        private readonly ISecurityService _testing;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public SecurityServiceTests( )
        {
            var mockJwtOptions = new Mock<IOptions<JwtOptions>>();

            var jwtOptions = new JwtOptions
            {
                Audience = "http://moodytest-env.eba-mmzgp9iv.eu-central-1.elasticbeanstalk.com/",
                Issuer = "http://moodytest-env.eba-mmzgp9iv.eu-central-1.elasticbeanstalk.com/",
                SecretKey = "2hN70OoacUi5SDU0rNuIXg==",
                TokenExpirationMinutes = 1440
            };

            mockJwtOptions.Setup(x => x.Value).Returns(jwtOptions);
        
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _testing = new SecurityService(mockJwtOptions.Object);
        }

        [Fact]
        public void GenerateJWtToken_ShoudlGenerateJwtTokenBasedOnGivenParameters()
        {
            var id = "1";
            var fullName = "random someone";

            var tokenInfo = _testing.GenerateJwtToken(id, fullName, UserTypeConstants.Admin);
            var jwtToken =  _jwtSecurityTokenHandler.ReadJwtToken(tokenInfo.Token);

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
            var id = "1";
            var fullName = "random someone";

            var tokenInfo = _testing.GenerateJwtToken(id, fullName, UserTypeConstants.Admin);
            var jwtToken =  _jwtSecurityTokenHandler.ReadJwtToken(tokenInfo.Token);

            Assert.InRange(jwtToken.ValidTo, DateTime.UtcNow.AddMinutes(1430), DateTime.UtcNow.AddMinutes(1450));
        }
    }
}

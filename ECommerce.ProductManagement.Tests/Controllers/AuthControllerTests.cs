#region using directives

using ECommerce.ProductManagement.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

#endregion

namespace ECommerce.ProductManagement.Tests.Controllers
{
    /// <summary>
    /// Unit tests for the AuthController class, verifying authentication and JWT token generation behavior.
    /// </summary>
    public class AuthControllerTests
    {
        private IConfiguration BuildConfig()
        {
            var inMem = new Dictionary<string, string>
            {
                // Use a short, human-friendly secret to exercise the PBKDF2 fallback in AuthController
                // (the controller will derive a sufficiently long key when the configured secret is short).
                ["Jwt:Key"] = "SuperSecretKey12345",
                ["Jwt:Issuer"] = "TestIssuer"
            };

            return new ConfigurationBuilder().AddInMemoryCollection(inMem).Build();
        }

        /// <summary>
        /// Verifies that Login returns 401 Unauthorized when invalid credentials are provided.
        [Fact]
        public void Login_ReturnsUnauthorized_ForInvalidCredentials()
        {
            var config = BuildConfig();
            var controller = new AuthController(config);

            var user = new User { Username = "bad", Password = "bad" };

            var result = controller.Login(user);

            Assert.IsType<UnauthorizedResult>(result);
        }

        /// <summary>
        /// Verifies that Login returns 200 OK with a JWT token containing the Admin role claim 
        /// when valid admin credentials are provided. Also checks that the user object is updated with the Admin role.
        /// </summary>
        [Fact]
        public void Login_Admin_ReturnsToken_WithAdminRoleClaim()
        {
            var config = BuildConfig();
            var controller = new AuthController(config);

            var user = new User { Username = AppConstants.AdminUserName, Password = AppConstants.AdminPassword };

            var result = controller.Login(user);

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = ok.Value!;
            var prop = value.GetType().GetProperty("token");
            Assert.NotNull(prop);

            var tokenString = (string)prop.GetValue(value)!;
            Assert.False(string.IsNullOrWhiteSpace(tokenString));

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenString);

            // verify issuer and claims
            Assert.Equal(config["Jwt:Issuer"], jwt.Issuer);
            Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.UniqueName || c.Type == System.Security.Claims.ClaimTypes.Name);
            Assert.Contains(jwt.Claims, c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == AppConstants.AdminRole);

            // the controller also sets the role on the passed user object
            Assert.Equal(AppConstants.AdminRole, user.Role);
        }

        /// <summary>
        /// Verifies that Login returns 200 OK with a JWT token containing the User role claim 
        /// when valid user credentials are provided. Also checks that the user object is updated with the User role.
        /// </summary>
        [Fact]
        public void Login_User_ReturnsToken_WithUserRoleClaim()
        {
            var config = BuildConfig();
            var controller = new AuthController(config);

            var user = new User { Username = AppConstants.UserName, Password = AppConstants.UserPassword };

            var result = controller.Login(user);

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = ok.Value!;
            var prop = value.GetType().GetProperty("token");
            Assert.NotNull(prop);

            var tokenString = (string)prop.GetValue(value)!;
            Assert.False(string.IsNullOrWhiteSpace(tokenString));

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenString);

            Assert.Equal(config["Jwt:Issuer"], jwt.Issuer);
            Assert.Contains(jwt.Claims, c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == AppConstants.UserRole);

            Assert.Equal(AppConstants.UserRole, user.Role);
        }
    }
}

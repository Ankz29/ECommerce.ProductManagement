using ECommerce.ProductManagement.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Security.Cryptography;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User login)
    {
        // Validate user (for demo, hardcoded)
        if (login.Username == AppConstants.AdminUserName && login.Password == AppConstants.AdminPassword)
        {
            login.Role = AppConstants.AdminRole;
        }
        else if (login.Username == AppConstants.UserName && login.Password == AppConstants.UserPassword)
        {
            login.Role = AppConstants.UserRole;
        }
        else
        {
            return Unauthorized();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login.Username),
            new Claim(ClaimTypes.Role, login.Role)
        };

        var configuredKey = _config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(configuredKey))
            throw new InvalidOperationException("JWT key is not configured.");

        byte[] keyBytes;
        try
        {
            // Try treating configured value as Base64
            keyBytes = Convert.FromBase64String(configuredKey);
        }
        catch (FormatException)
        {
            // Fallback to UTF-8 bytes for legacy plain-text secrets
            keyBytes = Encoding.UTF8.GetBytes(configuredKey);
        }

        // Ensure key is large enough for HS256 (must be > 256 bits)
        if (keyBytes.Length * 8 <= 256)
        {
            // If the configured secret is too short, derive a sufficiently long key using PBKDF2.
            // This allows short (human-friendly) secrets to be expanded into a secure key while
            // preserving the original secret as the derivation input. Prefer replacing the
            // configuration with a long Base64 secret in production.
            using (var deriveBytes = new Rfc2898DeriveBytes(configuredKey, Encoding.UTF8.GetBytes("ECommerceJwtKeySalt"), 10000, HashAlgorithmName.SHA256))
            {
                // Derive 64 bytes (512 bits) which is > 256 bits requirement.
                keyBytes = deriveBytes.GetBytes(64);
            }
        }

        var key = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

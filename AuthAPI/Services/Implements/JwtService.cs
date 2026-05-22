using AuthAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace AuthAPI.Services.Implements
{
    public class JwtService
    {
        private readonly IConfiguration config;

        public JwtService(IConfiguration config)
        {
            this.config = config;
        }

        public AuthResponse GenerateToken(string email, string employeeId, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,email),
                new Claim("employeeId",employeeId),
                new Claim("role",role)
            };

            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( config["Jwt:Key"]));

            var credentials =
                new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiry = DateTime.UtcNow.AddMinutes(int.Parse(config["Jwt:DurationInMinutes"]));

            var jwt = new JwtSecurityToken(
                            issuer: config["Jwt:Issuer"],
                            audience: config["Jwt:Audience"],
                            claims: claims,
                            expires: expiry,
                            signingCredentials: credentials
                        );

            string refreshToken = GenerateRefreshToken();

            DateTime refreshExpiry = DateTime.UtcNow.AddDays(7);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),

                Expiration = expiry,

                RefreshToken = refreshToken,

                RefreshTokenExpiry = refreshExpiry,

                RoleType = role
            };
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}

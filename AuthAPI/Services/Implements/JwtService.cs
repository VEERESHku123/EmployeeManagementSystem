using AuthAPI.Data.Repos.Abstracts;
using AuthAPI.DTOs.Common;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Services.Implements
{
    public class JwtService
    {
        private readonly IConfiguration config;
        private readonly IEmployeeRepo employeeRepo;

        public JwtService(IConfiguration config, IEmployeeRepo employeeRepo)
        {
            this.config = config;
            this.employeeRepo = employeeRepo;
        }

        public async Task<AuthResponse> GenerateToken(string email, string employeeId, string role)
        {
            var employee = await employeeRepo.CheckEmailExistsAsync(email);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email,email),
                new Claim("employeeId",employeeId),
                new Claim(ClaimTypes.Name, employee.FirstName),
                new Claim(ClaimTypes.Role,role)
            };

            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( config["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiry = DateTime.Now.AddMinutes(int.Parse(config["Jwt:DurationInMinutes"]));

            var jwt = new JwtSecurityToken(
                            issuer: config["Jwt:Issuer"],
                            audience: config["Jwt:Audience"],
                            claims: claims,
                            expires: expiry,
                            signingCredentials: credentials
                        );

            string refreshToken = GenerateRefreshToken();

            DateTime refreshExpiry = DateTime.Now.AddDays(5);

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

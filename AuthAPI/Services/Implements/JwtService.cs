using AuthAPI.Data.Repos.Abstracts;
using AuthAPI.DTOs.Common;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

            var expiry = DateTime.UtcNow.AddSeconds(int.Parse(config["Jwt:DurationInMinutes"]));

            var jwt = new JwtSecurityToken(
                            issuer: config["Jwt:Issuer"],
                            audience: config["Jwt:Audience"],
                            claims: claims,
                            expires: expiry,
                            signingCredentials: credentials
                        );

            string refreshToken = GenerateRefreshToken(email, employeeId);

            DateTime refreshExpiry = DateTime.UtcNow.AddDays(5);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),

                Expiration = expiry,

                RefreshToken = refreshToken,

                RefreshTokenExpiry = refreshExpiry,
                RoleType = role
            };
        }

        private string GenerateRefreshToken(string email, string employeeId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("employeeId", employeeId),
                new Claim("tokenType", "Refresh"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(
                    refreshToken,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["Jwt:Key"])),

                        ValidateIssuer = true,
                        ValidIssuer = config["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = config["Jwt:Audience"],

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken);

                var tokenType = principal.FindFirst("tokenType")?.Value;

                if (tokenType != "Refresh")
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}

using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Implements;
using AuthAPI.Data.Repos.Interfaces;
using AuthAPI.DTOs;
using AuthAPI.Services.Interfaces;

namespace AuthAPI.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly IRoleRepo roleRepo;
        private readonly JwtService jwtService;

        public UserService(IUserRepo userRepo, JwtService jwtService, IEmployeeRepo employeeRepo, IRoleRepo roleRepo)
        {
            this.userRepo = userRepo;
            this.jwtService = jwtService;
            this.employeeRepo = employeeRepo;
            this.roleRepo = roleRepo;
        }

        public async Task<LoginResponse> Login(LoginDto loginDto)
        {
            try
            {
                var user = await userRepo.GetUserByEmail(loginDto.Email);

                if (user == null) return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };

                bool passwordMatch = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

                if (!passwordMatch) return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };

                var authResponse = jwtService.GenerateToken(user.Email, user.EmployeeId, user.Role.RoleName);

                await userRepo.SaveRefreshToken(user.UserId, authResponse.RefreshToken, authResponse.RefreshTokenExpiry);

                return new LoginResponse
                {
                    Success = true,
                    Message = "Login successful",
                    AuthResponse = authResponse
                };


            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<LoginResponse> MicrosoftLogin(MicrosoftSignInRequest request)
        {
            try
            {
                var user = await userRepo.GetUserByEmail(request.Email);

                if (user == null) return new LoginResponse
                {
                    Success = false,
                    Message = "User not registered"
                };

                var authResponse = jwtService.GenerateToken(user.Email, user.EmployeeId, user.Role.RoleName);

                await userRepo.SaveRefreshToken(user.UserId, authResponse.RefreshToken, authResponse.RefreshTokenExpiry);

                return new LoginResponse
                {
                    Success = true,
                    Message = "Login successful",
                    AuthResponse = authResponse
                };

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<(bool success, string message)> ActivateAccount(LoginDto loginDto)
        {
            try
            {
                var exisistinguser = await userRepo.GetUserByEmail(loginDto.Email);

                if (exisistinguser != null) return (false, "Account already activated");

                var employee = await employeeRepo.CheckEmailExistsAsync(loginDto.Email);

                if (employee == null) return (false, "Employee not found");

                var role = await roleRepo.GetRoleByName("User");

                string hashPassword = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);

                var user = new UserEntity { Email = loginDto.Email, PasswordHash = hashPassword, EmployeeId = employee.EmployeeId, RefreshToken = "", RefreshTokenExpiryTime = null, RoleId = role.RoleId };

                var result = await userRepo.AddUser(user);

                return (result, "Account Activated");

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

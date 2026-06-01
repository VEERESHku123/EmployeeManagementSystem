using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Abstracts;
using AuthAPI.DTOs;
using AuthAPI.Services.Abstracts;

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
                if(user == null)
                {
                    var employee = await employeeRepo.CheckEmailExistsAsync(loginDto.Email);
                    if(employee == null) return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };

                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Please Active Your Account"
                    };
                }
               

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

        public async Task<ApiResponse<object>> ActivateAccount(LoginDto loginDto)
        {
            try
            {
                var existingUser =
                    await userRepo.GetUserByEmail(loginDto.Email);

                if (existingUser != null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Account already activated"
                    };
                }

                var employee =
                    await employeeRepo.CheckEmailExistsAsync(loginDto.Email);

                if (employee == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }

                var role =
                    await roleRepo.GetRoleByName("User");

                string hashPassword =
                    BCrypt.Net.BCrypt.HashPassword(loginDto.Password);

                var user = new UserEntity
                {
                    Email = loginDto.Email,
                    PasswordHash = hashPassword,
                    EmployeeId = employee.EmployeeId,
                    RefreshToken = "",
                    RefreshTokenExpiryTime = null,
                    RoleId = role.RoleId
                };

                var result =
                    await userRepo.AddUser(user);

                if (!result)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Activation failed"
                    };
                }

                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Account activated",
                    Data = null
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<AuthResponse>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var user = await userRepo.GetByRefreshToken(refreshTokenDto.RefreshToken);

            if (user == null) return new ApiResponse<AuthResponse>
            {
                Success = false,
                Message = "Refresh token expired"
            };

            if (user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                await userRepo.SaveRefreshToken(user.UserId, null, null);

                return new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Refresh token expired"
                };
            }

            var auth = jwtService.GenerateToken(user.Email, user.EmployeeId,user.Role.RoleName);

            await userRepo.SaveRefreshToken(user.UserId, auth.RefreshToken, auth.RefreshTokenExpiry);

            return new ApiResponse<AuthResponse>
            {
                Success = true,

                Message =  "Token refreshed successfully",

                Data = auth
            };

        }

        public async Task<ApiResponse<object>> SignOut(string? email)
        {
            try
            {

                if (string.IsNullOrEmpty(email))
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Email is required"
                    };
                }

                var user = await userRepo.GetUserByEmail(email);

                if (user == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                await userRepo.SaveRefreshToken(user.UserId, null, null);
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Logout successful"
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

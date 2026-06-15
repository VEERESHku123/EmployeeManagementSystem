using AuthAPI.Data.Entitys;
using AuthAPI.Data.Repos.Abstracts;
using AuthAPI.DTOs.Common;
using AuthAPI.DTOs.SigIn;
using AuthAPI.Services.Abstracts;
using System.Security.Claims;

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

        public async Task<SignInResponse> SignIn(SignInDto signInDto)
        {
            var validationResult = await ValidateEmployeeForLogin(signInDto.Email);

            if (!validationResult.IsValid)
            {
                return validationResult.Error!;
            }

            var employee = validationResult.Employee!;

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(signInDto.Password, employee.User.PasswordHash);

            if (!passwordMatch)
            {
                return new SignInResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            var role = await roleRepo.GetRoleById(employee.User.RoleId);

            var authResponse = await jwtService.GenerateToken(employee.CompanyEmail, employee.EmployeeId,role.RoleName);

            await userRepo.SaveRefreshToken(employee.User.UserId, authResponse.RefreshToken, authResponse.RefreshTokenExpiry);

            return new SignInResponse
            {
                Success = true,
                Message = "SignIn successful",
                AuthResponse = authResponse
            };
        }

        public async Task<SignInResponse> MicrosoftLogin(MicrosoftSignInRequest request)
        {
            try
            {
                var validationResult = await ValidateEmployeeForLogin(request.Email);

                if (!validationResult.IsValid)
                {
                    return validationResult.Error!;
                }

                var employee = validationResult.Employee!;

                var role = await roleRepo.GetRoleById(employee.User.RoleId);

                var authResponse = await jwtService.GenerateToken(employee.CompanyEmail, employee.EmployeeId, role.RoleName);

                await userRepo.SaveRefreshToken(employee.User.UserId, authResponse.RefreshToken, authResponse.RefreshTokenExpiry);

                return new SignInResponse
                {
                    Success = true,
                    Message = "SignIn successful",
                    AuthResponse = authResponse
                };

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ApiResponse<object>> ActivateAccount(ActivateAccountDto activateAccountDto)
        {
            try
            {
                var employee = await employeeRepo.CheckEmailExistsAsync(activateAccountDto.Email);


                if (employee.User.IsActive == true)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Account already activated"
                    };
                }


                if (employee == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }

                bool isValidTemporaryPassword = BCrypt.Net.BCrypt.Verify(activateAccountDto.TemporaryPassword, employee.User.PasswordHash);

                if (!isValidTemporaryPassword)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid Email or password"
                    };
                }

                var role = await roleRepo.GetRoleByName("Employee");

                string hashPassword = BCrypt.Net.BCrypt.HashPassword(activateAccountDto.Password);

                var user = new UserEntity
                {
                    PasswordHash = hashPassword,
                    EmployeeId = employee.EmployeeId,
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null,
                    RoleId = role.RoleId,
                    IsActive = true
                };

                var result = await userRepo.AddUser(user);

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
            var principal = jwtService.ValidateRefreshToken(refreshTokenDto.RefreshToken);

            if (principal == null)
            {
                return new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }

            var employeeId = principal.FindFirst("employeeId")?.Value;

            var user = await userRepo.GetUserByEmployeeId(employeeId);

            if (user == null)
            {
                return new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (user.RefreshToken != refreshTokenDto.RefreshToken)
            {
                return new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }

            if (!user.RefreshTokenExpiryTime.HasValue || user.RefreshTokenExpiryTime.Value <= DateTime.UtcNow)
            {
                await userRepo.SaveRefreshToken(user.UserId, null, null);

                return new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Refresh token expired"
                };
            }

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;

            var authResponse = await jwtService.GenerateToken(email, user.EmployeeId, user.Role.RoleName);

            await userRepo.SaveRefreshToken(user.UserId, authResponse.RefreshToken, authResponse.RefreshTokenExpiry);

            return new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Token refreshed successfully",
                Data = authResponse
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

                var employee = await employeeRepo.CheckEmailExistsAsync(email);

                if (employee == null || employee.User == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                await userRepo.SaveRefreshToken(employee.User.UserId, null, null);
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


        // Helper method

        private async Task<SignInValidationResult> ValidateEmployeeForLogin(string email)
        {
            var employee = await employeeRepo.CheckEmailExistsAsync(email);

            if (employee == null || employee.User == null)
            {
                return new SignInValidationResult
                {
                    Error = new SignInResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    }
                };
            }

            if (!employee.IsActive)
            {
                return new SignInValidationResult
                {
                    Error = new SignInResponse
                    {
                        Success = false,
                        Message = "Account has been deactivated"
                    }
                };
            }

            if (!employee.User.IsActive)
            {
                return new SignInValidationResult
                {
                    Error = new SignInResponse
                    {
                        Success = false,
                        Message = "Please activate your account"
                    }
                };
            }



            return new SignInValidationResult
            {
                Employee = employee
            };
        }
    }
}

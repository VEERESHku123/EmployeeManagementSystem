using Frontend.ApiServices.Abstracts;
using Frontend.Models.Common;
using Frontend.Models.Employee;
using System.Net;
using System.Net.Http.Headers;
namespace Frontend.ApiServices.Implements
{
    public class EmployeeApiService : BaseApiService, IEmployeeApiService
    {
        public EmployeeApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory, httpContextAccessor, "Backend") { }
        

        public async Task<ApiResponse<EmployeePaginationData>>GetAllEmployees(string searchTerm,int page,int pageSize)
        {
            var url = $"employee/all?search={Uri.EscapeDataString(searchTerm ?? "")}" + $"&page={page}&pageSize={pageSize}";

            var response = await SendAuthorizedRequestAsync(() => client.GetAsync(url));

            if (response == null)
            {
                return new ApiResponse<EmployeePaginationData>
                {
                    Success = false,
                    Message = "Session expired"
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<EmployeePaginationData>
                {
                    Success = false,
                    Message = "Failed to fetch employees"
                };
            }

            return await response.Content.ReadFromJsonAsync<ApiResponse<EmployeePaginationData>>()
                   ?? new ApiResponse<EmployeePaginationData>
                   {
                       Success = false,
                       Message = "No response"
                   };
        }

        public async Task<ApiResponse<EmployeeModel>> GetEmployeeById(string? employeeId)
        {
            var response = await SendAuthorizedRequestAsync( () => client.GetAsync($"employee/{employeeId}"));

            if (response == null)
            {
                return new ApiResponse<EmployeeModel>
                {
                    Success = false,
                    Message = "Session expired"
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                return new ApiResponse<EmployeeModel>
                {
                    Success = false,
                    Message = "Failed to fetch employee"
                };
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeModel>>();

            return result!;
        }

        public async Task<ApiResponse<EmployeeModel>> AddNewEmployee(EmployeeModel model)
        {
            var response = await SendAuthorizedRequestAsync(() => client.PostAsJsonAsync("employee/add",model));

            // Refresh token expired
            if (response == null)
            {
                return new ApiResponse<EmployeeModel>
                {
                    Success = false,
                    Message = "Session expired"
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeModel>>();

                return error ??
                       new ApiResponse<EmployeeModel>
                       {
                           Success = false,
                           Message = "Unable to create employee"
                       };
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<EmployeeModel>>();

            return result ??
                   new ApiResponse<EmployeeModel>
                   {
                       Success = true,
                       Message = "Employee created successfully",
                       Data = model
                      
                   };
        }

        public async Task<ApiResponse<UpdateEmployeeModel>> UpdateEmployee(string id, UpdateEmployeeModel model)
        {
            var response = await SendAuthorizedRequestAsync(() => client.PutAsJsonAsync($"employee/update/{id}", model));

            // Refresh token expired
            if (response == null)
            {
                return new ApiResponse<UpdateEmployeeModel>
                {
                    Success = false,
                    Message = "Session expired"
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<UpdateEmployeeModel>>();

                return error ??
                       new ApiResponse<UpdateEmployeeModel>
                       {
                           Success = false,
                           Message = "Unable to update employee"
                       };
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<UpdateEmployeeModel>>();

            return result ??
                   new ApiResponse<UpdateEmployeeModel>
                   {
                       Success = true,
                       Message = "Employee updated successfully",
                       Data = model
                   };
        }

        public async Task<ApiResponse<bool>> DeleteEmployee(string id)
        {
            var response = await SendAuthorizedRequestAsync(() => client.DeleteAsync($"employee/delete/{id}"));

            // Refresh token expired
            if (response == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Session expired"
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

                return error ??
                       new ApiResponse<bool>
                       {
                           Success = false,
                           Message = "Unable to delete employee"
                       };
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            return result ??
                   new ApiResponse<bool>
                   {
                       Success = true,
                       Message = "Employee deleted successfully",
                       Data = true
                   };
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            var token = httpContextAccessor.HttpContext?.Session.GetString("AccessToken");
            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);
            var response = await client.GetAsync($"employee/CheckEmailExists/{email}");

          
            return response.IsSuccessStatusCode;

        }

        public async Task<bool> CheckEmployeeIdExists(string employeeId)
        {
            var token = httpContextAccessor.HttpContext?.Session.GetString("AccessToken");

            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);
            var response = await client.GetAsync($"employee/CheckEmployeeIdExists/{employeeId}");

            

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckPhoneExists(string phoneNumber, string? employeeId)
        {
            var token = httpContextAccessor.HttpContext?.Session.GetString("AccessToken");

            client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Bearer",
                            token);

            var url = $"employee/CheckPhoneExists?phoneNumber={phoneNumber}&id={employeeId}";

            var response = await client.GetAsync(url);



            return response.StatusCode == HttpStatusCode.Conflict;
        }


        public async Task<ApiResponse<List<DesignationModel>>> GetAllDesignations()
        {
            var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"employee/designationList"));

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<DesignationModel>>>();

            return new ApiResponse<List<DesignationModel>>
            {
                Data = result.Data,
                Message = "Successfully Fetched"
            };
        }

        public async Task<ApiResponse<object>> UploadEmployeesAsync(IFormFile file)
        {
            try
            {
                using var content = new MultipartFormDataContent();

                using var stream = file.OpenReadStream();

                var fileContent = new StreamContent(stream);

                content.Add(fileContent,"file",file.FileName);

                var response = await SendAuthorizedRequestAsync(() => client.PostAsync("employee/upload-employees",content));
                Console.WriteLine("---------------------");
                Console.WriteLine(response.StatusCode);
                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Failed to upload employees."
                    };
                }

                var result =
                    await response.Content
                        .ReadFromJsonAsync<ApiResponse<object>>();

                return result!;
            }
            catch
            {
                throw;
            }
            
        }

        public async Task<byte[]> DownloadTemplateAsync()
        {
            var response = await SendAuthorizedRequestAsync(() => client.GetAsync("employee/download-template"));
          
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}

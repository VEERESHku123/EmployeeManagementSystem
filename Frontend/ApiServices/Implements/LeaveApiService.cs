using Frontend.ApiServices.Abstracts;
using Frontend.Models.Common;
using Frontend.Models.Leave;

namespace Frontend.ApiServices.Implements
{
    public class LeaveApiService : BaseApiService, ILeaveApiService
    {
        public LeaveApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory, httpContextAccessor, "Backend")
        {
        }

        public async Task<ApiResponse<List<LeaveBalanceModel>>> GetEmployeeLeaveBalancesAsync()
        {
            try
            {
                var response = await SendAuthorizedRequestAsync(() => client.GetAsync("leave/balances"));

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<LeaveBalanceModel>>>();

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<LeaveBalanceModel>>
                    {
                        Success = false,
                        Message = "Unable to load leave balances.",
                        Data = null
                    };
                }

                return result ?? new ApiResponse<List<LeaveBalanceModel>>
                {
                    Success = false,
                    Message = "Unable to parse API response.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<LeaveBalanceModel>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };

            }
        }

        public async Task<ApiResponse<List<LeaveHistoryModel>>> GetLeaveHistory(string status = "Pending")
        {
           
            var response = await SendAuthorizedRequestAsync(() => client.GetAsync($"leave/history/{status}"));

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<LeaveHistoryModel>>>();
            Console.WriteLine("-----------");
            result.Data.ForEach(e => Console.WriteLine(e.ManagerRemark));
            return result;
        }

        public async Task<ApiResponse<int>> ApplyLeaveAsync(ApplyLeaveModel model)
        {
            var response = await SendAuthorizedRequestAsync(() => client.PostAsJsonAsync("leave/apply", model));

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                return new ApiResponse<int>
                {
                    Success = false,
                    Message = error
                };
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();

            return result;
        }
    }
}

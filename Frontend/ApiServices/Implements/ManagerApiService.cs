using Azure.Core;
using Frontend.ApiServices.Abstracts;
using Frontend.Models.Common;
using Frontend.Models.Leave;
using Frontend.Models.Manager;

namespace Frontend.ApiServices.Implements
{
    public class ManagerApiService : BaseApiService, IManagerApiService
    {
        public ManagerApiService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor) : base(factory, httpContextAccessor, "Backend")
        {
        }

        public async Task<ApiResponse<List<LeaveRequestModel>>> GetTeamLeaveRequests()
        {
            var response = await SendAuthorizedRequestAsync(() => client.GetAsync("manager/leaveRequests"));

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<LeaveRequestModel>>>();

            return result;
        }

        public async Task<ApiResponse<string>> ApproveOrRejectLeaveAsync(LeaveApprovalRequestModel leaveApprovalRequest)
        {
            var response = await SendAuthorizedRequestAsync(() => client.PutAsJsonAsync("manager/leave/approve-reject", leaveApprovalRequest));

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result;
        }
    }
}

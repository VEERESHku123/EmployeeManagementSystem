using Frontend.ApiServices.Abstracts;
using Frontend.Models.Manager;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManagerApiService managerApiService;

        public ManagerController(IManagerApiService managerApiService)
        {
            this.managerApiService = managerApiService;
        }

        [HttpGet("manager/teamLeaveRequestes")]
        public async Task<IActionResult> TeamLeaveRequests()
        {
            var response = await managerApiService.GetTeamLeaveRequests();

            return PartialView("_TeamLeaveRequests", response.Data);
        }

        [HttpPost("manager/approveOrRejectLeave")]
        public async Task<IActionResult> ApproveOrRejectLeave([FromBody] LeaveApprovalRequestModel model)
        {
            var response = await managerApiService.ApproveOrRejectLeaveAsync(model);

            return Json(response);
        }
    }
}

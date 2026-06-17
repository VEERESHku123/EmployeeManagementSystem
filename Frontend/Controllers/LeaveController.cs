using Frontend.ApiServices.Abstracts;
using Frontend.Models.Leave;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeaveApiService leaveApiService;

        public LeaveController(ILeaveApiService leaveApiService)
        {
            this.leaveApiService = leaveApiService;
        }

        [HttpGet("leave/apply")]
        public async Task<IActionResult> Apply()
        {
            var balances = await leaveApiService.GetEmployeeLeaveBalancesAsync();

            var history = await leaveApiService.GetLeaveHistory("Pending");

            var model = new LeaveApplyViewModel
            {
                LeaveBalances = balances?.Data ?? new(),
                LeaveHistory = history?.Data ?? new()
            };

            return PartialView("Apply", model);
        }

        [HttpPost("leave/apply")]
        public async Task<IActionResult> Apply(LeaveApplyViewModel model)
        {
            var response =
                await leaveApiService.ApplyLeaveAsync(model.ApplyLeave);

            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> LeaveHistory(string status)
        {
            var response = await leaveApiService.GetLeaveHistory(status);

            return PartialView("_LeaveHistoryCards", response.Data);
        }

       
    }
}

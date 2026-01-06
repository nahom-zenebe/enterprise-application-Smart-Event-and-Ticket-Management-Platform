using Microsoft.AspNetCore.Mvc;

namespace SmartPlatform.Api.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController : ControllerBase
    {
        [HttpGet("events/{id}")]
        public IActionResult GetEventAnalytics(int id)
        {
            return Ok(new
            {
                EventId = id,
                TicketsSold = 120,
                Attendance = 98,
                Revenue = 4800
            });
        }

        [HttpGet("users/{id}")]
        public IActionResult GetUserAnalytics(int id)
        {
            return Ok(new
            {
                UserId = id,
                TicketsPurchased = 5,
                EventsAttended = 3,
                LastActive = DateTime.UtcNow.AddDays(-2)
            });
        }

        [HttpGet("revenue")]
        public IActionResult GetRevenueAnalytics()
        {
            return Ok(new
            {
                Daily = 1200,
                Monthly = 34000,
                Yearly = 410000
            });
        }
    }
}

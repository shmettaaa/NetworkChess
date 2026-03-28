using Microsoft.AspNetCore.Mvc;

namespace NetworkWebChess.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "Healthy",
                message = "Сервер работает))",
                timestamp = DateTime.UtcNow
            });
        }
    }
}

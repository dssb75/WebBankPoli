using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Source = "Original",
                Timestamp = DateTime.Now,
                Message = "Respuesta desde AccountService"
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace AccountServiceReplica.Controllers
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
                Source = "Replica",
                Timestamp = DateTime.Now,
                Message = "Respuesta desde AccountServiceReplica"
            });
        }
    }
}

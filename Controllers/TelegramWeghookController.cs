using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace QualityControl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelegramWeghookController : Controller
    {
        private readonly ILogger<TelegramWeghookController> _logger;
        
        public TelegramWeghookController(ILogger<TelegramWeghookController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] JObject update)
        {
            ArgumentNullException.ThrowIfNull(HttpContext.Connection.RemoteIpAddress);
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            _logger.LogInformation($"Получено обновления от IP: {ipAddress}");

            return Ok();
        }
    }
}

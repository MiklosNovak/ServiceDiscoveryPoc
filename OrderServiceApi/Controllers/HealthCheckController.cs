using Microsoft.AspNetCore.Mvc;

namespace OrderServiceApi.Controllers;

[ApiController]
public class HealthCheckController : ControllerBase
{
    [HttpGet("/health", Name = "HealthCheck")]
    public IActionResult Get()
    {
        return Ok();
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Sidecar.Controllers;

[ApiController]
[Route("services")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ServiceRegistryController : ControllerBase
{
    private readonly ServiceRegistry _serviceRegistry;

    public ServiceRegistryController(ServiceRegistry serviceRegistry)
    {
        _serviceRegistry = serviceRegistry;
    }

    [HttpPost(Name = "RegisterService")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterServiceAsync([FromBody]RegistrationRequest req)
    {
        await _serviceRegistry.RegisterServiceAsync(req.Name, req.Address, req.Port).ConfigureAwait(false);
        return Ok($"{req.Name} registered!");
    }

    [HttpGet(Name = "GetService")]
    [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceAsync([FromQuery, Required] string serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return BadRequest("Service name is required.");
        }

        var service = await _serviceRegistry.GetServiceAsync(serviceName).ConfigureAwait(false);

        if (service == null)
        {
            return NotFound($"Service {serviceName} not found.");
        }

        var response = new RegistrationResponse
        {
            Address = service.Address,
            Name = service.Service,
            Port = service.Port
        };

        return Ok(response);
    }
}
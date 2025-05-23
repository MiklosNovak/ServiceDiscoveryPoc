using Microsoft.AspNetCore.Mvc;
using OrderServiceApi.Dtos;
using System.Net.Mime;

namespace OrderServiceApi.Controllers;

[ApiController]
[Route("orders")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class OrderController : ControllerBase
{
    private readonly ProductRepository _productRepository;

    public OrderController(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }


    [HttpPost(Name = "CreateOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateOrderAsync([FromBody]OrderCreateRequest orderCreateRequest)
    {
        var product = _productRepository.GetProduct(orderCreateRequest.Sku);

        if (product == null)
        {
            var allProducts = _productRepository.GetProducts();
            return NotFound($"Product {orderCreateRequest.Sku} not found! Available products: {string.Join(",", allProducts)}");
        }

        //todo::
        // check quantity by using inventory service
        // if quantity is not available, return 422 Unprocessable Entity

        // todo:: add sidecar registration

        //todo:: create inventory service

        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;
using OrderServiceApi.Dtos;
using OrderServiceApi.Inventory;
using OrderServiceApi.Products;
using System.Net.Mime;

namespace OrderServiceApi.Controllers;

[ApiController]
[Route("orders")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class OrderController : ControllerBase
{
    private readonly ProductRepository _productRepository;
    private readonly InventoryClient _inventoryClient;

    public OrderController(ProductRepository productRepository, InventoryClient inventoryClient)
    {
        _productRepository = productRepository;
        _inventoryClient = inventoryClient;
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

        var amounts = await _inventoryClient.GetAllAsync().ConfigureAwait(false);

        if (!amounts.TryGetValue(orderCreateRequest.Sku, out var availableQuantity) || availableQuantity < orderCreateRequest.Quantity)
        {
            return UnprocessableEntity($"Insufficient quantity for product {orderCreateRequest.Sku}. Available: {availableQuantity}, Requested: {orderCreateRequest.Quantity}");
        }

        return Ok();
    }
}
using System.ComponentModel.DataAnnotations;

namespace OrderServiceApi.Dtos;

public record OrderCreateRequest
{
    [Required]
    public string Sku { get; init; }

    [Required, Range(0, 100)]
    public int Quantity { get; init; }
}
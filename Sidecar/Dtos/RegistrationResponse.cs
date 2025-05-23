using System.ComponentModel.DataAnnotations;

namespace Sidecar.Dtos;

public record RegistrationResponse
{
    [Required]
    public string Name { get; init; }

    [Required]
    public string Address { get; init; }

    public int Port { get; init; }
}
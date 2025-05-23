namespace OrderServiceApi.ServiceRegistry.Dtos;

public record RegistrationRequest
{
    public string Name { get; init; }

    public string Address { get; init; }

    public int Port { get; init; }
}
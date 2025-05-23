namespace OrderServiceApi.ServiceRegistry.Dtos;

public record RegistrationResponse
{
    public string Name { get; init; }

    public string Address { get; init; }

    public int Port { get; init; }
}
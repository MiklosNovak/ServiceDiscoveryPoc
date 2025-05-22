using Consul;

namespace Sidecar;

public class ServiceRegistry
{
    private readonly IConsulClient _consulClient;

    public ServiceRegistry(IConsulClient consulClient)
    {
        _consulClient = consulClient;
    }

    public async Task RegisterServiceAsync(string serviceName, string address, int port)
    {
        var registration = new AgentServiceRegistration
        {
            ID = $"{serviceName}-{Guid.NewGuid()}",
            Name = serviceName,
            Address = address,
            Port = port,
            Check = new AgentServiceCheck
            {
                HTTP = $"http://{address}:{port}/health",
                Interval = TimeSpan.FromSeconds(10),
                Timeout = TimeSpan.FromSeconds(5),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
            }
        };

        await _consulClient.Agent.ServiceRegister(registration).ConfigureAwait(false);
    }

    public async Task<AgentService> GetServiceAsync(string serviceName)
    {
        var queryResult = await _consulClient.Agent.Services().ConfigureAwait(false);

        if (queryResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception($"Failed to query Consul for service {serviceName}. Status code: {queryResult.StatusCode}");
        }

        var service = queryResult.Response?
            .Where(s => string.Equals(s.Value.Service, serviceName, StringComparison.InvariantCultureIgnoreCase))
            .Select(s => s.Value);

        return service?.FirstOrDefault();
    }
}
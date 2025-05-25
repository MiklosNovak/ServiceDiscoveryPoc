using Consul;

namespace Sidecar;

public class ServiceRegistry
{
    private readonly IConsulClient _consulClient;
    private readonly Dictionary<string, AgentService> _serviceCache = new(StringComparer.InvariantCultureIgnoreCase);

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
        var service = await TryGetServiceAsync(serviceName).ConfigureAwait(false);

        if (service != null)
        {
            _serviceCache[serviceName] = service;
        }

        return service;
    }

    private async Task<AgentService> TryGetServiceAsync(string serviceName)
    {
        try
        {
            var queryResult = await _consulClient.Agent.Services().ConfigureAwait(false);

            if (queryResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Failed to query Consul for service {serviceName}. Status code: {queryResult.StatusCode}");
            }

            var serviceQuery = queryResult.Response?
                .Where(s => string.Equals(s.Value.Service, serviceName, StringComparison.InvariantCultureIgnoreCase))
                .Select(s => s.Value);

            return serviceQuery?.FirstOrDefault();
        }
        catch
        {
            // Simple caching mechanism which can help somewhat if the Consul server is down or unreachable
            if (_serviceCache.TryGetValue(serviceName, out var cachedService))
            {
                return cachedService;
            }
        }

        return null;
    }
}
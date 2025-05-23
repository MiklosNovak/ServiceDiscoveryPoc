namespace OrderServiceApi.ServiceRegistry;

using System;
using System.Threading.Tasks;
using Dtos;
using RestSharp;

public class ServiceRegistryClient
{
    private readonly RestClient _client;

    public ServiceRegistryClient(IConfiguration configuration)
    {
        var baseUrl = configuration.GetValue<Uri>("SideCarUrl");
        _client = new RestClient(baseUrl);
    }

    public async Task RegisterServiceAsync(RegistrationRequest request)
    {
        var restRequest = new RestRequest("services", Method.Post)
            .AddJsonBody(request);

        var response = await _client.ExecuteAsync(restRequest);

        if (!response.IsSuccessful)
        {
            throw new Exception("Failed to register service: " + response.Content);
        }
    }

    public async Task<RegistrationResponse> GetServiceAsync(string serviceName)
    {
        var restRequest = new RestRequest("services")
            .AddQueryParameter("serviceName", serviceName);

        var response = await _client.ExecuteAsync<RegistrationResponse>(restRequest);

        if (!response.IsSuccessful)
        {
            throw new Exception("Failed to get service: " + response.Content);
        }

        return response.Data;
    }
}
using RestSharp;

namespace OrderServiceApi.Inventory;

public class InventoryClient
{
    private readonly RestClient _client;

    public InventoryClient(Uri inventoryClientUrl)
    {
        _client = new RestClient(inventoryClientUrl);
    }

    public async Task<Dictionary<string, int>> GetAllAsync()
    {
        var restRequest = new RestRequest("inventory");

        var response = await _client.ExecuteAsync<Dictionary<string, int>>(restRequest).ConfigureAwait(false);

        if (!response.IsSuccessful)
        {
            throw new Exception("Failed to get service: " + response.Content);
        }

        return response.Data;
    }
}
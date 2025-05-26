using OrderServiceApi.Inventory;
using OrderServiceApi.Products;
using OrderServiceApi.ServiceRegistry;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddSingleton<ServiceRegistryClient>();

builder.Services.AddScoped(s =>
{
   var serviceRegistryClient =  s.GetRequiredService<ServiceRegistryClient>();
   var service =  serviceRegistryClient.GetServiceAsync("InventoryServiceApi");
   var serviceUrl = $"http://{service.Result.Address}:{service.Result.Port}";
   return new InventoryClient(new(serviceUrl));
});

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Lifetime.ApplicationStarted.Register(async () =>
{
    var registryClient = app.Services.GetRequiredService<ServiceRegistryClient>();

    var address = Dns.GetHostName();
    var port = 8080; // Kestrel listens on port 8080 by default

    await registryClient.RegisterServiceAsync(new()
    {
        Name = "OrderServiceApi",
        Address = address,
        Port = port
    }).ConfigureAwait(false);
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

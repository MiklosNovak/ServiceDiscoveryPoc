using Consul;
using Sidecar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

builder.Services.AddSingleton<IConsulClient, ConsulClient>(s => new (cfg =>
{
    var config = s.GetRequiredService<IConfiguration>();
    var consulUrl = config.GetValue<string>("ConsulUrl");
    cfg.Address = new Uri(consulUrl);
}));

builder.Services.AddSingleton<ServiceRegistry>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

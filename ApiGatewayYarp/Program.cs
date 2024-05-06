using DomainService.Configurations;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
// Adding Authentication
builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();
//app.MapGet("/", () => "Hello World!");
app.UseAuthentication();

app.UseAuthorization();

app.Run();

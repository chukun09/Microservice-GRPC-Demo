using AuthMicroservice.Services;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using CodeBaseAPI.DependencyInjection;
using Core.Entites;
using Data;
using DomainService.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using EntityFrameworkCore.Seed;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
ConfigurationManager configuration = builder.Configuration;
// For Entity Framework
builder.Services.AddDbContext<AuthenticationDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("Connection"), options => options.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName));

    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);

// For Identity
builder.Services.AddIdentity<UserEntity, IdentityRole>()
    .AddEntityFrameworkStores<AuthenticationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});
// Adding Authentication
builder.Services.AddJwtAuthentication(configuration);

// Add GRPC Service
builder.Services.AddGRPC(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name);

// Setup RabbitMQ
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddEventBusServiceAuthenticationService(builder.Configuration);

builder.Services.AddMediatR();
//builder.Services.Configure<EventBusSettings>(options => builder.Configuration.GetSection(nameof(EventBusSettings)).Bind(options));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
//Using autofac for Dependency Injection
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new ContainerConfigModule());
});
var app = builder.Build();


var dbInitService = app.Services.GetService<IDbInitializer>();
if (dbInitService != null) await dbInitService.SeedUsers();
// Use Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
app.MapGrpcService<UserService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();


using AttendanceMicroservice.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodeBaseAPI.DependencyInjection;
using Data;
using DomainService.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
ConfigurationManager configuration = builder.Configuration;
// For Entity Framework
builder.Services.AddDbContext<AttendanceDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("Connection"), options => options.MigrationsAssembly(typeof(AttendanceDbContext).Assembly.FullName));

    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);
// Adding Authentication
builder.Services.AddJwtAuthentication(configuration);
// Add GRPC Service
builder.Services.AddGRPC(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name);

// Setup RabbitMQ
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddEventBusServiceAttendanceService(builder.Configuration);

builder.Services.AddMediatR();
//builder.Services.Configure<EventBusSettings>(options => builder.Configuration.GetSection(nameof(EventBusSettings)).Bind(options));
builder.Services.AddAutoMapper(typeof(Startup).Assembly);
//builder.Services.AddAutoMapper()
builder.Services.AddHttpContextAccessor();
//Using autofac for Dependency Injection
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new ContainerConfigModule());
});
var app = builder.Build();

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
app.MapGrpcService<AttendanceService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

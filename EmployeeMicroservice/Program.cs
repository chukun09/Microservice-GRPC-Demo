using EmployeeMicroservice.Services;
using DomainService.Configurations;
using Serilog;
using Data;
using Microsoft.EntityFrameworkCore;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using CodeBaseAPI.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Service.Middleware;
using Core.Helpers;
using Demo.gRPC.Interceptors;
using DepartmentMicroservice.Services;

if (Log.Logger is not Serilog.Core.Logger)
{
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        //.WriteTo.Sink() // By default we only log to the console
        .CreateLogger();
}
var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;


// For Entity Framework
builder.Services.AddDbContext<EmployeeDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("Connection"), options => options.MigrationsAssembly(typeof(EmployeeDbContext).Assembly.FullName));

    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);
// Adding Authentication
builder.Services.AddJwtAuthentication(configuration);
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
builder.Services.AddGRPC(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name);

// Setup RabbitMQ
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddEventBusServiceEmployeeService(builder.Configuration);
builder.Services.AddMediatR();

//builder.Services.Configure<EventBusSettings>(options => builder.Configuration.GetSection(nameof(EventBusSettings)).Bind(options));

builder.Services.AddAutoMapper(typeof(Startup).Assembly);
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
// Configure the HTTP request pipeline.
//app.UseMiddleware<ErrorHandlerMiddleware>();
// Configure the HTTP request pipeline.
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapGrpcService<EmployeeService>();
app.MapGrpcService<DepartmentService>();
app.MapGrpcService<WorkHoursSummaryService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

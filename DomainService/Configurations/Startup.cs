using Demo.gRPC.Interceptors;
using DomainService.Consumer;
using EventBus.Common;
using EventBus.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Configurations
{
    public static class Startup
    {
        public static IServiceCollection AddGRPC(this IServiceCollection services, string filePath, string fileName)
        {
            // Add services to the container.
            services.AddGrpc(options =>
            {
                {
                    options.Interceptors.Add<ExceptionInterceptor>();
                    options.EnableDetailedErrors = true;
                }
            }).AddJsonTranscoding();

            // Config Swagger for gRPC
            services.AddGrpcSwagger();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
                // Add Comment from gRPC Comment
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{fileName}.xml";
                var xmlPath = Path.Combine(filePath, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.IncludeGrpcXmlComments(xmlPath, includeControllerXmlComments: true);
            });
            return services;
        }
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, ConfigurationManager configuration)
        {
            // Adding Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization();
            return services;
        }

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Information($"Configuring settings for application.");

            // Configure settings for the event bus
            services.Configure<EventBusSettings>(options => configuration.GetSection(nameof(EventBusSettings)).Bind(options));

            return services;
        }

        public static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            Log.Information($"Configuring mediatR for application.");

            // Configure settings for the event bus
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            return services;
        }



        public static IServiceCollection AddEventBusServiceAttendanceService(this IServiceCollection services, IConfiguration configuration)
        {
            // Get the event bus settings from the configuration
            EventBusSettings? settings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

            if (settings == null)
            {
                throw new NullReferenceException("The Event Bus Settings has not been configured. Please check the settings and update them.");
            }

            Log.Information($"Registering MassTransit Service for Attendance.");

            services.AddMassTransit(config =>
            {
                //config.AddConsumer<UserAttendancedConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(settings.Host, x =>
                    {
                        x.Username(settings.Username);
                        x.Password(settings.Password);
                    });
                    //cfg.ReceiveEndpoint(EventBusConstants.UserAttendancedEvent, c =>
                    //{
                    //    c.ConfigureConsumer<UserAttendancedConsumer>(ctx);
                    //});
                });
            });

            return services;
        }
        public static IServiceCollection AddEventBusServiceEmployeeService(this IServiceCollection services, IConfiguration configuration)
        {
            // Get the event bus settings from the configuration
            EventBusSettings? settings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

            if (settings == null)
            {
                throw new NullReferenceException("The Event Bus Settings has not been configured. Please check the settings and update them.");
            }

            Log.Information($"Registering MassTransit Service for Employeee.");

            services.AddMassTransit(config =>
            {
                config.AddConsumer<UserAttendancedConsumer>();
                config.AddConsumer<UserLoggedInConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(settings.Host, x =>
                    {
                        x.Username(settings.Username);
                        x.Password(settings.Password);
                    });
                    cfg.ReceiveEndpoint(EventBusConstants.UserAttendancedEvent, c =>
                    {
                        c.ConfigureConsumer<UserAttendancedConsumer>(ctx);
                    });
                    cfg.ReceiveEndpoint(EventBusConstants.UserLoggedinEvent, c =>
                    {
                        c.ConfigureConsumer<UserLoggedInConsumer>(ctx);
                    });
                });
            });
            services.AddScoped<UserAttendancedConsumer>();
            services.AddScoped<UserLoggedInConsumer>();
            return services;
        }
        public static IServiceCollection AddEventBusServiceAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            // Get the event bus settings from the configuration
            EventBusSettings? settings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

            if (settings == null)
            {
                throw new NullReferenceException("The Event Bus Settings has not been configured. Please check the settings and update them.");
            }

            Log.Information($"Registering MassTransit Service for Authentication.");

            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(settings.Host, x =>
                    {
                        x.Username(settings.Username);
                        x.Password(settings.Password);
                    });
                });
            });

            return services;
        }
    }
}
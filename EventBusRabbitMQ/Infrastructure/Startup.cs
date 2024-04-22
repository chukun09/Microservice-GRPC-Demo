//using MassTransit;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Serilog;

//namespace EventBusRabbitMQ.Infrastructure
//{
//    public static class Startup
//    {
//        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
//        {
//            Log.Information($"Configuring settings for application.");

//            // Configure settings for the event bus
//            services.Configure<EventBusSettings>(options => configuration.GetSection(nameof(EventBusSettings)).Bind(options));

//            return services;
//        }


//        public static IServiceCollection AddEventBusServiceAttendanceService(this IServiceCollection services, IConfiguration configuration)
//        {
//            // Get the event bus settings from the configuration
//            EventBusSettings? settings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

//            if (settings == null)
//            {
//                throw new NullReferenceException("The Event Bus Settings has not been configured. Please check the settings and update them.");
//            }

//            Log.Information($"Registering MassTransit Service for Attendance.");

//            services.AddMassTransit(config =>
//            {
//                config.UsingRabbitMq((ctx, cfg) =>
//                {
//                    cfg.Host(settings.Host, x =>
//                    {
//                        x.Username(settings.Username);
//                        x.Password(settings.Password);
//                    });
//                });
//            });

//            return services;
//        }        public static IServiceCollection AddEventBusServiceEmployeeService(this IServiceCollection services, IConfiguration configuration)
//        {
//            // Get the event bus settings from the configuration
//            EventBusSettings? settings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

//            if (settings == null)
//            {
//                throw new NullReferenceException("The Event Bus Settings has not been configured. Please check the settings and update them.");
//            }

//            Log.Information($"Registering MassTransit Service for Employeee.");

//            services.AddMassTransit(config =>
//            {
//                config.AddConsumer<UserAttendanceConsumer>();
//                config.UsingRabbitMq((ctx, cfg) =>
//                {
//                    cfg.Host(settings.Host, x =>
//                    {
//                        x.Username(settings.Username);
//                        x.Password(settings.Password);
//                    });
//                });
//            });

//            return services;
//        }        public static IServiceCollection AddEventBusServiceAuthenticationService(this IServiceCollection services, IConfiguration configuration)
//        {
//            // Get the event bus settings from the configuration
//            EventBusSettings? settings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

//            if (settings == null)
//            {
//                throw new NullReferenceException("The Event Bus Settings has not been configured. Please check the settings and update them.");
//            }

//            Log.Information($"Registering MassTransit Service for Authentication.");

//            services.AddMassTransit(config =>
//            {
//                config.UsingRabbitMq((ctx, cfg) =>
//                {
//                    cfg.Host(settings.Host, x =>
//                    {
//                        x.Username(settings.Username);
//                        x.Password(settings.Password);
//                    });
//                });
//            });

//            return services;
//        }
//    }
//}


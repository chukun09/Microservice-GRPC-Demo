using Grpc.Net.Client;
using Grpc.Core;
using EmployeeMicroservice;
using DepartmentMicroservice;
using WorkHoursSummaryMicroservice;
using AttendanceMicroservice;
using AuthMicroservice.Protos;
using AuthenticationWithClientSideBlazor.Client;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebAppBlazor.Services.Authentication;

namespace WebAppBlazor.Services
{
    public static class Startup
    {
        public static IServiceCollection AddGRPCClient(this IServiceCollection services)
        {
            services.AddScoped<ITokenProvider, AppTokenProvider>();
            // Set URI Channel
            var channelAttendanceURI = new Uri("https://localhost:7217");
            var channelEmployeeURI = new Uri("https://localhost:7072");
            var channelAuthenticationURI = new Uri("https://localhost:7051");
            //services.AddSingleton(channelAttendance);
            //services.AddSingleton(channelEmployee);
            //services.AddSingleton(channelAuthentication);

            // Inject channel into each gRPC Employee client
            services.AddGrpcClient<Employeer.EmployeerClient>(options =>
            {
                options.Address = channelEmployeeURI;
            }).AddCallCredentials(async (context, metadata, serviceProvider) =>
            {
                var provider = serviceProvider.GetRequiredService<ITokenProvider>();
                var token = await provider.GetTokenAsync(context.CancellationToken);
                metadata.Add("Authorization", $"Bearer {token}");
            });
            services.AddGrpcClient<Departmenter.DepartmenterClient>(options =>
            {
                options.Address = channelEmployeeURI;
            }).AddCallCredentials(async (context, metadata, serviceProvider) =>
            {
                var provider = serviceProvider.GetRequiredService<ITokenProvider>();
                var token = await provider.GetTokenAsync(context.CancellationToken);
                metadata.Add("Authorization", $"Bearer {token}");
            });
            services.AddGrpcClient<WorkHoursSummaryService.WorkHoursSummaryServiceClient>(options =>
            {
                options.Address = channelEmployeeURI;
            }).AddCallCredentials(async (context, metadata, serviceProvider) =>
            {
                var provider = serviceProvider.GetRequiredService<ITokenProvider>();
                var token = await provider.GetTokenAsync(context.CancellationToken);
                metadata.Add("Authorization", $"Bearer {token}");
            });
            // Inject channel into each gRPC Attandance client
            services.AddGrpcClient<Attendancer.AttendancerClient>(options =>
            {
                options.Address = channelAttendanceURI;
            }).AddCallCredentials(async (context, metadata, serviceProvider) =>
            {
                var provider = serviceProvider.GetRequiredService<ITokenProvider>();
                var token = await provider.GetTokenAsync(context.CancellationToken);
                metadata.Add("Authorization", $"Bearer {token}");
            });
            // Inject channel into each gRPC Authentication client
            services.AddGrpcClient<Userer.UsererClient>(options =>
            {
                options.Address = channelAuthenticationURI;
            });
            return services;
        }
        public static IServiceCollection AddAuthenticationService(this IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}

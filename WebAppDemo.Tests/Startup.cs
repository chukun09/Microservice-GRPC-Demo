using Autofac;
using Autofac.Extensions.DependencyInjection;
using CodeBaseAPI.DependencyInjection;
using Core.Entites;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebAppDemo.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            services.AddHttpContextAccessor();
            services.AddDbContext<EmployeeDbContext>(options =>
             {
                 options.UseNpgsql("Server=localhost;uid=postgres;password=12345678;port=5432;database=EmployeeDatabase;");

             }
                );
            services.AddDbContext<AuthenticationDbContext>(options =>
             {
                 options.UseNpgsql("Server=localhost;uid=postgres;password=12345678;port=5432;database=AuthenticationDatabase;");

             }
                );
            services.AddDbContext<AttendanceDbContext>(options =>
             {
                 options.UseNpgsql("Server=localhost;uid=postgres;password=12345678;port=5432;database=AttendanceDatabase;");

             }
                );
            services.AddIdentity<UserEntity, IdentityRole>()
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();
        }

        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory())
         .ConfigureContainer<ContainerBuilder>(builder =>
         {
             builder.RegisterModule(new ContainerConfigModule());
         });
        }
    }
}
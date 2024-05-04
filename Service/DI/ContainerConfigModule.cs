using Autofac;
using System.Reflection;
using System.Collections.Generic;
using Core.IRepositories;
using Data.Repositories;
using DomainService.UnitOfWorks;
using DomainService.UnitOfWorks.Interfaces;
using EntityFrameworkCore.Seed;
using AutoMapper;

namespace CodeBaseAPI.DependencyInjection
{
    public class ContainerConfigModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(UnitOfWork<>)).As(typeof(IUnitOfWork<>)).InstancePerLifetimeScope();
            builder.RegisterType<AttendanceUnitOfWork>().As<IAttendanceUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationUnitOfWork>().As<IAuthenticationUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<EmployeeUnitOfWork>().As<IEmployeeUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbInitializer>().As<IDbInitializer>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.Load("Service")).Where(t => t.Name.Contains("Service")).As(t => t.GetInterfaces().FirstOrDefault(x => x.Name == "I" + t.Name)).InstancePerLifetimeScope();

        }
    }
}

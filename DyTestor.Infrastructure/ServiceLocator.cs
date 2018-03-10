using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DyTestor.Infrastructure
{
    public static class ServiceLocator
    {
        private static IContainer autofacContainer;
        static ServiceLocator()
        {
            ContainerBuilder builder = new ContainerBuilder();

            Assembly Domain = Assembly.Load("DyTestor.Domain");
            Assembly Application = Assembly.Load("DyTestor.Application");
            Assembly Repositories = Assembly.Load("DyTestor.Repositories");
            Assembly ServiceContracts = Assembly.Load("DyTestor.ServiceContracts");

            builder.RegisterAssemblyTypes(Repositories);
            builder.RegisterAssemblyTypes(Application, ServiceContracts).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Domain);

            autofacContainer = builder.Build();
        }

        public static T GetService<T>()
        {
            return autofacContainer.Resolve<T>();
        }
    }
}

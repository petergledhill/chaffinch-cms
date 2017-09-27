using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using CQRSlite.Routing;

namespace Core.DI
{
    public static class CQRSLiteBuilderExtensions
    {
        public static IApplicationBuilder UseCQRSLiteBus(this IApplicationBuilder builder, params Type[] typesFromAssemblyContainingMessages)
        {
            var provider = new HttpProvider(builder.ApplicationServices.GetService<IServiceProvider>());
            var registrar = new RouteRegistrar(provider);
            registrar.Register(typesFromAssemblyContainingMessages);
            return builder;
        }
    }
}

using Core.DI;
using CQRSlite.Caching;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Routing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionsExtensions
    {
        public static CqrsLiteBuilder AddCQRSLite(this IServiceCollection services)
        {
            services.AddSingleton<Router>(new Router());
            services.AddSingleton<ICommandSender>(y => y.GetService<Router>());
            services.AddSingleton<IEventPublisher>(y => y.GetService<Router>());
            services.AddSingleton<IHandlerRegistrar>(y => y.GetService<Router>());

            services.AddScoped<ISession, Session>();
            services.AddScoped<ICache, MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
 
            return new CqrsLiteBuilder(services);
        }
    }
}
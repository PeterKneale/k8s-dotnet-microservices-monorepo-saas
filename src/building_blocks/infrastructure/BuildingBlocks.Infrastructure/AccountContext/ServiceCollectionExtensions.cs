using System.Diagnostics.CodeAnalysis;
using BuildingBlocks.Infrastructure.AccountContext.Interceptors;
using BuildingBlocks.Infrastructure.AccountContext.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.AccountContext
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// For frontends, backends and services to store the ambient account context
        /// </summary>
        public static IServiceCollection AddScopedAccountContext(this IServiceCollection services) =>
            services
                .AddScoped<IAccountContextSetter, ScopedSetter>()
                .AddScoped<IAccountContextGetter, ScopedGetter>()
                .AddScoped<IScopedAccountContextStore, ScopedStore>();

        /// <summary>
        /// For frontends to extract the ambient account context and pass it to backends
        /// For backends to extract the ambient account context and pass it to services
        /// </summary>
        public static IServiceCollection AddAccountContextAwareClientInterceptor(this IServiceCollection services) =>
            services.AddScoped<AccountContextAwareClientInterceptor>();

        /// For backends to extract the account context from a call and store it as the ambient context (and subsequently pass to services)
        /// For services to extract the account context from a call and store it as the ambient context
        public static IServiceCollection AddAccountContextAwareServerInterceptor(this IServiceCollection services) =>
            services.AddScoped<AccountContextAwareServerInterceptor>();
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Route256.PriceCalculator.Domain.Interfaces.Repository;
using Route256.PriceCalculator.Domain.Options;
using Route256.PriceCalculator.Infrastructure.HostedServices;
using Route256.PriceCalculator.Infrastructure.Repositories;

namespace Route256.PriceCalculator.Infrastructure.Connectors;

public static class InfrastructureConnctors
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IDeliveriesRepository, DeliveriesRepository>();
        services.AddSingleton<IGoodsRepository, GoodsRepository>();

        return services;
    }

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<GoodsSyncHostedService>();

        return services;
    }

    public static IServiceCollection AddConfigurations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PriceCalculatorOptions>(configuration.GetSection("PriceCalculatorOptions"));

        return services;
    }
}

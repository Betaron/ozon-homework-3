using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Route256.PriceCalculator.Domain.Interfaces.Repositories;
using Route256.PriceCalculator.Domain.Interfaces.Services;
using Route256.PriceCalculator.Domain.Options;
using Route256.PriceCalculator.Domain.Services;
using Route256.PriceCalculator.Infrastructure.HostedServices;
using Route256.PriceCalculator.Infrastructure.Repositories;

namespace Route256.PriceCalculator.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IDeliveryPriceCalculatorService, DeliveryPriceCalculatorService>();
        services.AddScoped<IGoodPriceCalculatorService, GoodPriceCalculatorService>();
        services.AddScoped<IGoodsService, GoodsService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddHostedServices();
        services.AddConfigurations(configuration);

        return services;
    }

    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IDeliveriesRepository, DeliveriesRepository>();
        services.AddSingleton<IGoodsRepository, GoodsRepository>();

        return services;
    }

    internal static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<GoodsSyncHostedService>();

        return services;
    }

    internal static IServiceCollection AddConfigurations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PriceCalculatorOptions>(configuration.GetSection("PriceCalculatorOptions"));

        return services;
    }
}

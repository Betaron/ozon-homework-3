using Microsoft.Extensions.DependencyInjection;
using Route256.PriceCalculator.Domain.Interfaces.Service;
using Route256.PriceCalculator.Domain.Services;

namespace Route256.PriceCalculator.Infrastructure.Connectors;

public static class DomainConnector
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IDeliveryPriceCalculatorService, DeliveryPriceCalculatorService>();
        services.AddScoped<IGoodPriceCalculatorService, GoodPriceCalculatorService>();
        services.AddScoped<IGoodsService, GoodsService>();

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Route256.PriceCalculator.Domain.Interfaces.Repositories;
using Route256.PriceCalculator.Domain.Interfaces.Services;

namespace Route256.PriceCalculator.Infrastructure.HostedServices;

internal sealed class GoodsSyncHostedService : BackgroundService
{
    private readonly IGoodsRepository _repository;
    private readonly IServiceProvider _serviceProvider;

    public GoodsSyncHostedService(
        IGoodsRepository repository,
        IServiceProvider serviceProvider)
    {
        _repository = repository;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var goodsService = scope.ServiceProvider.GetRequiredService<IGoodsService>();
                var goods = goodsService.GetGoodsWithRandomCount().ToList();
                foreach (var good in goods)
                    _repository.AddOrUpdate(good);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
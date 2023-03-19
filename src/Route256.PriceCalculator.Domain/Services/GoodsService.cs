using Route256.PriceCalculator.Domain.Interfaces.Repository;
using Route256.PriceCalculator.Domain.Interfaces.Service;
using Route256.PriceCalculator.Domain.Models;

namespace Route256.PriceCalculator.Domain.Services;

internal sealed class GoodsService : IGoodsService
{
    private readonly IGoodsRepository _goodsRepository;

    public GoodsService(IGoodsRepository goodsRepository)
    {
        _goodsRepository = goodsRepository;
    }

    public IEnumerable<GoodModel> GetAll() =>
        _goodsRepository.GetAll();

    public IEnumerable<GoodModel> GetGoodsWithRandomCount()
    {
        var rnd = new Random();
        foreach (var model in _goodsRepository.GetAll())
        {
            var count = rnd.Next(0, 10);
            yield return model with { Count = count };
        }
    }
}

using Route256.PriceCalculator.Domain.Interfaces.Repositories;
using Route256.PriceCalculator.Domain.Interfaces.Services;

namespace Route256.PriceCalculator.Domain.Services;

internal sealed class GoodPriceCalculatorService : IGoodPriceCalculatorService
{
    private readonly IGoodsRepository _goodsRepository;
    private readonly IDeliveryPriceCalculatorService _deliveryPriceCalculatorService;

    public GoodPriceCalculatorService(
        IGoodsRepository goods,
        IDeliveryPriceCalculatorService deliveryPriceCalculatorService)
    {
        _goodsRepository = goods;
        _deliveryPriceCalculatorService = deliveryPriceCalculatorService;
    }

    public decimal CalculatePrice(int goodId, int distance = 0)
    {
        if (goodId == default)
        {
            throw new ArgumentException($"{nameof(goodId)} is default");
        }

        if (!_goodsRepository.ContainsById(goodId))
        {
            throw new ArgumentOutOfRangeException(
                nameof(goodId),
                message: $"Good with id {goodId} does not exist");
        }

        var good = _goodsRepository.Get(goodId);

        return _deliveryPriceCalculatorService.CalculatePrice(good, distance);
    }
}
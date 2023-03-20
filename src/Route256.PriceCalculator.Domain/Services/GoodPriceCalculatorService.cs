using Route256.PriceCalculator.Domain.Interfaces.Repository;
using Route256.PriceCalculator.Domain.Interfaces.Service;

namespace Route256.PriceCalculator.Domain.Services;

internal sealed class GoodPriceCalculatorService : IGoodPriceCalculatorService
{
    private readonly IGoodsRepository _goods;
    private readonly IDeliveryPriceCalculatorService _deliveryPriceCalculatorService;

    public GoodPriceCalculatorService(
        IGoodsRepository goods,
        IDeliveryPriceCalculatorService deliveryPriceCalculatorService)
    {
        _goods = goods;
        _deliveryPriceCalculatorService = deliveryPriceCalculatorService;
    }

    public decimal CalculatePrice(int goodId, int distance = 0)
    {
        if (goodId == default)
            throw new ArgumentException($"{nameof(goodId)} is default");

        var good = _goods.Get(goodId);

        return _deliveryPriceCalculatorService.CalculatePrice(good, distance);
    }
}
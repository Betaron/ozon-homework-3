namespace Route256.PriceCalculator.Domain.Interfaces.Service;

using Route256.PriceCalculator.Domain.Models;

public interface IDeliveryPriceCalculatorService
{
    DeliveryModel[] QueryLog(int take);
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods, int distance = 0);
    decimal CalculatePrice(GoodModel good, int distance = 0);
}
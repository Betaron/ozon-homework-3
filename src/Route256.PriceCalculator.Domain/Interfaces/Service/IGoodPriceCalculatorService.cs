namespace Route256.PriceCalculator.Domain.Interfaces.Service;

public interface IGoodPriceCalculatorService
{
    decimal CalculatePrice(int goodId, int distance = 0);
}
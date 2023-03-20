namespace Route256.PriceCalculator.Domain.Interfaces.Service;

public interface IGoodPriceCalculatorService
{
    /// <summary>
    /// Вычисляет стоимось доставки товара, 
    /// основываясь на его характеристиках и расстоянии доставки
    /// </summary>
    /// <param name="goodId">Идентификатор товара в хранилище</param>
    /// <param name="distance">Расстояние доставки товара</param>
    /// <returns></returns>
    decimal CalculatePrice(int goodId, int distance = 0);
}
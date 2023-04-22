namespace Route256.PriceCalculator.Domain.Interfaces.Services;

using Route256.PriceCalculator.Domain.Models;

public interface IDeliveryPriceCalculatorService
{
    /// <summary>
    /// Делает запрос к хранилищу с логами расчетов стоимостей доставок.
    /// </summary>
    /// <param name="take">Количество возвращаемых записей из логов</param>
    DeliveryModel[] QueryLog(int take);

    /// <summary>
    /// Вычисляет стоимость доставки основываясь на:
    /// весе, объеме товаров, а также на расстоянии доставки.
    /// </summary>
    /// <param name="goods">Список доставляемых товаров</param>
    /// <param name="distance">Расстояние, на которое доставляется товар</param>
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods, int distance = 0);

    /// <summary>
    /// Вычисляет стоимость доставки основываясь на:
    /// весе, объеме товара, а также на расстоянии доставки.
    /// </summary>
    /// <param name="goods">Товар для оценки стоимости доставки</param>
    /// <param name="distance">Расстояние, на которое доставляется товар</param>
    decimal CalculatePrice(GoodModel good, int distance = 0);
}
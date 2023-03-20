using Route256.PriceCalculator.Domain.Models;

namespace Route256.PriceCalculator.Domain.Interfaces.Service;

public interface IGoodsService
{
    /// <summary>
    /// Получает список товаров и остатки на складах.
    /// </summary>
    IEnumerable<GoodModel> GetAll();

    /// <summary>
    /// Получает список товаров и случайные остатки на складах.
    /// </summary>
    IEnumerable<GoodModel> GetGoodsWithRandomCount();
}
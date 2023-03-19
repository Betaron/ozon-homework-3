using Route256.PriceCalculator.Domain.Models;

namespace Route256.PriceCalculator.Domain.Interfaces.Service;

public interface IGoodsService
{
    IEnumerable<GoodModel> GetAll();
    IEnumerable<GoodModel> GetGoodsWithRandomCount();
}
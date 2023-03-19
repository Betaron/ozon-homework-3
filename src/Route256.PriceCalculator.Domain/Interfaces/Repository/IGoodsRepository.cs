namespace Route256.PriceCalculator.Domain.Interfaces.Repository;

using Route256.PriceCalculator.Domain.Models;

public interface IGoodsRepository
{
    void AddOrUpdate(GoodModel entity);
    ICollection<GoodModel> GetAll();
    GoodModel Get(int id);
    bool ContainsById(int id);
}
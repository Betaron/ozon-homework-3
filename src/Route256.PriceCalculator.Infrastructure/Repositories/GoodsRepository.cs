using Route256.PriceCalculator.Domain.Interfaces.Repositories;
using Route256.PriceCalculator.Domain.Models;
using Route256.PriceCalculator.Infrastructure.Entities;

namespace Route256.PriceCalculator.Infrastructure.Repositories;

internal sealed class GoodsRepository : IGoodsRepository
{
    private readonly List<GoodEntity> _storage = new()
    {
        new("Парик для питомца", 1, 1000, 2000, 3000, 4000, 0, 100),
        new("Накидка на телевизор", 2, 1000, 2000, 3000, 4000, 0, 120),
        new("Ковёр настенный", 3, 2000, 3000, 3000, 5000, 0, 140),
        new("Здоровенный ЯЗЬ", 4, 1000, 1000, 4000, 4000, 0, 160),
        new("Билет МММ", 5, 3000, 2000, 1000, 5000, 0, 180)
    };

    public void AddOrUpdate(GoodModel model)
    {
        var good = _storage.FirstOrDefault(x => x.Id == model.Id);
        if (good != null)
        {
            _storage.Remove(good);
        }

        _storage.Add(new GoodEntity(model));
    }

    public ICollection<GoodModel> GetAll() =>
        _storage.Select(x => x.ToGoodModel()).ToArray();

    public GoodModel Get(int id) =>
        _storage.First(x => x.Id == id).ToGoodModel();

    public bool ContainsById(int id) =>
        _storage.Any(x => x.Id == id);
}
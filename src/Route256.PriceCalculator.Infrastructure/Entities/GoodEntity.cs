namespace Route256.PriceCalculator.Infrastructure.Entities;

using Route256.PriceCalculator.Domain.Models;

internal sealed record GoodEntity(
    string Name,
    int Id,
    int Height,
    int Length,
    int Width,
    int Weight,
    int Count,
    decimal Price
)
{
    public GoodEntity(GoodModel goodModel) : this(
        goodModel.Name,
        goodModel.Id,
        goodModel.Height,
        goodModel.Length,
        goodModel.Width,
        goodModel.Weight,
        goodModel.Count,
        goodModel.Price)
    { }

    public GoodModel ToGoodModel() =>
        new GoodModel(
            Name,
            Id,
            Height,
            Length,
            Width,
            Weight,
            Count,
            Price);
}

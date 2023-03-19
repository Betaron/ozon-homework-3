namespace Route256.PriceCalculator.Domain.Models;

public record GoodModel(
    string Name = "",
    int Id = 0,
    int Height = 0,
    int Length = 0,
    int Width = 0,
    int Weight = 0,
    int Count = 0,
    decimal Price = 0);
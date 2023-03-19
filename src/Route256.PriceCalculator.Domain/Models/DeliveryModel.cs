namespace Route256.PriceCalculator.Domain.Models;

public record DeliveryModel(
    DateTime At = default,
    decimal Volume = 0,
    decimal Weight = 0,
    decimal Distance = 0,
    decimal Price = 0);
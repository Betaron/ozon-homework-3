namespace Route256.PriceCalculator.Infrastructure.Entities;

using Route256.PriceCalculator.Domain.Models;

internal record DeliveryEntity(
    DateTime At,
    decimal Volume,
    decimal Weight,
    decimal Distance,
    decimal Price)
{
    public DeliveryEntity(DeliveryModel model) : this(
        model.At,
        model.Volume,
        model.Weight,
        model.Distance,
        model.Price)
    { }

    public DeliveryModel ToDeliveryModel() =>
        new DeliveryModel(
            At,
            Volume,
            Weight,
            Distance,
            Price);
}

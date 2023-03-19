namespace Route256.PriceCalculator.Domain.Interfaces.Repository;

using Route256.PriceCalculator.Domain.Models;

public interface IDeliveriesRepository
{
    void Save(DeliveryModel entity);
    IReadOnlyList<DeliveryModel> Query();
}
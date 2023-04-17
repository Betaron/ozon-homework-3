namespace Route256.PriceCalculator.Domain.Interfaces.Repositories;

using Route256.PriceCalculator.Domain.Models;

public interface IDeliveriesRepository
{
    void Save(DeliveryModel entity);
    IReadOnlyList<DeliveryModel> Query();
}
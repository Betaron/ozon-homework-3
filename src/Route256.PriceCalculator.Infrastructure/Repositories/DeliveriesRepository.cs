using Route256.PriceCalculator.Domain.Interfaces.Repository;
using Route256.PriceCalculator.Domain.Models;
using Route256.PriceCalculator.Infrastructure.Entities;

namespace Route256.PriceCalculator.Infrastructure.Repositories;

public class DeliveriesRepository : IDeliveriesRepository
{
    private readonly List<DeliveryEntity> _deliveries = new();

    public void Save(DeliveryModel model)
    {
        _deliveries.Add(new DeliveryEntity(model));
    }

    public IReadOnlyList<DeliveryModel> Query() =>
        _deliveries.Select(x => x.ToDeliveryModel()).ToList();
}
using Microsoft.Extensions.Options;
using Route256.PriceCalculator.Domain.Interfaces.Repository;
using Route256.PriceCalculator.Domain.Interfaces.Service;
using Route256.PriceCalculator.Domain.Models;
using Route256.PriceCalculator.Domain.Options;

namespace Route256.PriceCalculator.Domain.Services;

internal sealed class DeliveryPriceCalculatorService : IDeliveryPriceCalculatorService
{
    private readonly decimal _volumeToPriceRatio;
    private readonly decimal _weightToPriceRatio;
    private readonly decimal _distanceToPriceRatio;

    private readonly IDeliveriesRepository _deliveriesRepository;

    public DeliveryPriceCalculatorService(
        IOptionsSnapshot<PriceCalculatorOptions> options,
        IDeliveriesRepository storageRepository)
    {
        _volumeToPriceRatio = options.Value.VolumeToPriceRatio;
        _weightToPriceRatio = options.Value.WeightToPriceRatio;
        _distanceToPriceRatio = options.Value.DistanceToPriceRatio;
        _deliveriesRepository = storageRepository;
    }

    public decimal CalculatePrice(IReadOnlyList<GoodModel> goods, int distance)
    {
        if (!goods.Any())
        {
            throw new ArgumentOutOfRangeException(nameof(goods));
        }

        var volumePrice = CalculatePriceByVolume(goods, out var volume);
        var weightPrice = CalculatePriceByWeight(goods, out var weight);

        var resultPrice = ApplyDistanceToPrice(Math.Max(volumePrice, weightPrice), distance);

        _deliveriesRepository.Save(new DeliveryModel(
            DateTime.UtcNow,
            volume,
            weight,
            distance,
            resultPrice));

        return resultPrice;
    }

    public decimal CalculatePrice(GoodModel good, int distance) =>
        CalculatePrice(new List<GoodModel>() { good }, distance);

    private decimal CalculatePriceByVolume(
    IReadOnlyList<GoodModel> goods,
    out decimal volume)
    {
        volume = goods
            .Select(x => x.Height * x.Width * x.Length / 1000m)
            .Sum();

        return volume * _volumeToPriceRatio;
    }

    private decimal CalculatePriceByWeight(
        IReadOnlyList<GoodModel> goods,
        out decimal weight)
    {
        weight = goods
            .Select(x => x.Weight / 1000m)
            .Sum();

        return weight * _weightToPriceRatio;
    }

    public DeliveryModel[] QueryLog(int take)
    {
        if (take == 0)
        {
            return Array.Empty<DeliveryModel>();
        }

        var log = _deliveriesRepository.Query()
            .OrderByDescending(x => x.At)
            .Take(take)
            .ToArray();

        return log
            .Select(x => new DeliveryModel(
                x.At,
                x.Volume,
                x.Weight,
                x.Distance,
                x.Price))
            .ToArray();
    }

    private decimal ApplyDistanceToPrice(decimal price, decimal distance)
    {
        if (distance == 0)
            return price;

        return price * distance * _distanceToPriceRatio;
    }
}
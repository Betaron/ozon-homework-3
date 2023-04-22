namespace Route256.PriceCalculator.Domain.Options;

public sealed class PriceCalculatorOptions
{
    public decimal VolumeToPriceRatio { get; set; }
    public decimal WeightToPriceRatio { get; set; }
    public decimal DistanceToPriceRatio { get; set; }
}
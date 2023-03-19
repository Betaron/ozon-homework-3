namespace Route256.PriceCalculator.Domain.Options;

internal sealed class PriceCalculatorOptions
{
    public decimal VolumeToPriceRatio { get; set; }
    public decimal WeightToPriceRatio { get; set; }
    public decimal DistanceToPriceRatio { get; set; }
}
using Route256.PriceCalculator.Api.Requests.V2;

namespace Route256.PriceCalculator.Api.Requests.V3;

public record CalculateRequest(GoodProperties[] Goods, int Distance);
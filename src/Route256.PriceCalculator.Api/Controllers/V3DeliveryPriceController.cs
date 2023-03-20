using Microsoft.AspNetCore.Mvc;
using Route256.PriceCalculator.Api.Requests.V3;
using Route256.PriceCalculator.Api.Responses.V3;
using Route256.PriceCalculator.Domain.Interfaces.Service;
using Route256.PriceCalculator.Domain.Models;

namespace Route256.PriceCalculator.Api.Controllers;

[ApiController]
public class V3DeliveryPriceController : Controller
{
    private readonly IGoodPriceCalculatorService _goodPriceCalculatorService;
    private readonly IDeliveryPriceCalculatorService _deliveryPriceCalculatorService;

    public V3DeliveryPriceController(
        IGoodPriceCalculatorService goodPriceCalculatorService,
        IDeliveryPriceCalculatorService deliveryPriceCalculatorService)
    {
        _goodPriceCalculatorService = goodPriceCalculatorService;
        _deliveryPriceCalculatorService = deliveryPriceCalculatorService;
    }

    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// или веса товара. Окончательная стоимость принимается как наибольшая
    /// </summary>
    /// <param name="request">Включает в себя список товаров</param>
    [HttpPost("calculate")]
    public CalculateResponse Calculate(CalculateRequest request)
    {
        var price = _deliveryPriceCalculatorService.CalculatePrice(
            request.Goods.Select(
                x => new GoodModel(
                    Height: x.Height,
                    Length: x.Length,
                    Width: x.Width,
                    Weight: x.Weight
                )).ToList(),
            (int)request.Distance);

        return new CalculateResponse(price);
    }

    /// <summary>
    /// Вычисляет стоимость доставки для товара с учетом расстояния.
    /// </summary>
    /// <param name="request">Включает товар и расстояние</param>
    [HttpPost("good/calculate")]
    public Task<CalculateResponse> Calculate(GoodCalculateRequest request)
    {
        var price = _goodPriceCalculatorService.CalculatePrice(request.GoodId, (int)request.Distance);

        return Task.FromResult(new CalculateResponse(price));
    }
}
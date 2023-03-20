using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Route256.PriceCalculator.Api.Requests.V2;
using Route256.PriceCalculator.Api.Responses.V2;
using Route256.PriceCalculator.Api.Validators;
using Route256.PriceCalculator.Domain.Interfaces.Service;
using Route256.PriceCalculator.Domain.Models;

namespace Route256.PriceCalculator.Api.Controllers;

[ApiController]
[Route("/v2/[controller]")]
public class V2DeliveryPriceController : Controller
{
    private readonly IDeliveryPriceCalculatorService _deliveryPriceCalculatorService;

    public V2DeliveryPriceController(
        IDeliveryPriceCalculatorService deliveryPriceCalculatorService)
    {
        _deliveryPriceCalculatorService = deliveryPriceCalculatorService;
    }

    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// или веса товара. Окончательная стоимость принимается как наибольшая
    /// </summary>
    /// <param name="request">Включает в себя список товаров</param>
    [HttpPost("calculate")]
    public async Task<CalculateResponse> Calculate(CalculateRequest request)
    {
        var validator = new CalculateRequestValidator();
        await validator.ValidateAndThrowAsync(request);

        var price = _deliveryPriceCalculatorService.CalculatePrice(
            request.Goods
                .Select(x => new GoodModel(
                    Height: x.Height,
                    Length: x.Length,
                    Width: x.Width,
                    Weight: x.Weight))
                .ToArray());

        return new CalculateResponse(price);
    }
}
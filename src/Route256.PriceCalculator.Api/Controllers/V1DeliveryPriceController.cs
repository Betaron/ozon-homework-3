using Microsoft.AspNetCore.Mvc;
using Route256.PriceCalculator.Api.Requests.V1;
using Route256.PriceCalculator.Api.Responses.V1;
using Route256.PriceCalculator.Domain.Interfaces.Service;
using Route256.PriceCalculator.Domain.Models;

namespace Route256.PriceCalculator.Api.Controllers;

[ApiController]
[Route("/v1/[controller]")]
public class V1DeliveryPriceController : ControllerBase
{
    private readonly IDeliveryPriceCalculatorService _deliveryPriceCalculatorService;

    public V1DeliveryPriceController(
        IDeliveryPriceCalculatorService deliveryPriceCalculatorService)
    {
        _deliveryPriceCalculatorService = deliveryPriceCalculatorService;
    }


    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// </summary>
    /// <returns></returns>
    [HttpPost("calculate")]
    public CalculateResponse Calculate(
        CalculateRequest request)
    {
        var price = _deliveryPriceCalculatorService.CalculatePrice(
            request.Goods
                .Select(x => new GoodModel(
                    Height: x.Height,
                    Length: x.Length,
                    Width: x.Width))
                .ToArray());

        return new CalculateResponse(price);
    }

    /// <summary>
    /// Метод получения истории вычисления
    /// </summary>
    /// <returns></returns>
    [HttpPost("get-history")]
    public GetHistoryResponse[] History(GetHistoryRequest request)
    {
        var log = _deliveryPriceCalculatorService.QueryLog(request.Take);

        return log
            .Select(x => new GetHistoryResponse(
                new CargoResponse(
                    x.Volume,
                    x.Weight),
                x.Price))
            .ToArray();
    }
}
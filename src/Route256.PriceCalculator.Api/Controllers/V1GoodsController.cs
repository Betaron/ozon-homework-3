using Microsoft.AspNetCore.Mvc;
using Route256.PriceCalculator.Api.Responses.V2;
using Route256.PriceCalculator.Domain.Interfaces.Service;
using Route256.PriceCalculator.Domain.Models;

namespace Route256.PriceCalculator.Api.Controllers;

[Route("goods")]
[ApiController]
public sealed class V1GoodsController
{
    private readonly IGoodsService _goodsService;
    private readonly IGoodPriceCalculatorService _goodPriceCalculatorService;

    public V1GoodsController(
        IGoodsService goodsService,
        IGoodPriceCalculatorService goodPriceCalculatorService)
    {
        _goodsService = goodsService;
        _goodPriceCalculatorService = goodPriceCalculatorService;
    }

    [HttpGet]
    public ICollection<GoodModel> GetAll()
    {
        return _goodsService.GetAll().ToList();
    }

    [HttpGet("calculate/{id}")]
    public CalculateResponse CalculateDelivery(int id)
    {
        var price = _goodPriceCalculatorService.CalculatePrice(id);
        return new CalculateResponse(price);
    }
}
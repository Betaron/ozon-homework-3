using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Moq;
using Route256.PriceCalculator.Domain.Interfaces.Repositories;
using Route256.PriceCalculator.Domain.Models;
using Route256.PriceCalculator.Domain.Options;
using Route256.PriceCalculator.Domain.Services;
using Xunit;

namespace PriceCalculator.UnitTests.Domain.Services;

public class GoodPriceCalculatorServiceTests
{
    private static Mock<IDeliveriesRepository> CreateDelivriesRepositoryMock()
    {
        var repositoryMock = new Mock<IDeliveriesRepository>(MockBehavior.Strict);
        repositoryMock.Setup(x => x.Save(It.IsAny<DeliveryModel>()));
        return repositoryMock;
    }

    private static IOptionsSnapshot<PriceCalculatorOptions> CreateUnitOptionsSnapshot()
    {
        var optionsMock = new Mock<IOptionsSnapshot<PriceCalculatorOptions>>(MockBehavior.Strict);

        optionsMock
            .Setup(x => x.Value)
            .Returns(() => new()
            {
                VolumeToPriceRatio = 1,
                WeightToPriceRatio = 1,
                DistanceToPriceRatio = 1
            });

        return optionsMock.Object;
    }

    [Fact]
    public void GoodPriceCalculatorService_WhenCalculatingGoodsIdDefault_ShouldThrow()
    {
        // Arrange
        var deliveryCalculator = new DeliveryPriceCalculatorService
            (CreateUnitOptionsSnapshot(), CreateDelivriesRepositoryMock().Object);
        var goodsRepository = new Mock<IGoodsRepository>();
        var cut = new GoodPriceCalculatorService(
            goodsRepository.Object, deliveryCalculator);

        // Assert, Act
        Assert.Throws<ArgumentException>(() => cut.CalculatePrice(default));
    }

    [Fact]
    public void GoodPriceCalculatorService_WhenCalculatingGoodDoesntExists_ShouldThrow()
    {
        // Arrange
        var deliveryCalculator = new DeliveryPriceCalculatorService
            (CreateUnitOptionsSnapshot(), CreateDelivriesRepositoryMock().Object);
        var goodsRepository = new Mock<IGoodsRepository>();
        goodsRepository.Setup(x => x.ContainsById(1)).Returns(false);
        var cut = new GoodPriceCalculatorService(
            goodsRepository.Object, deliveryCalculator);

        // Assert, Act
        Assert.Throws<ArgumentOutOfRangeException>(() => cut.CalculatePrice(1));
    }

    [Theory]
    [MemberData(nameof(CalcManyGoodsMemberData))]
    public void GoodPriceCalculatorService_WhenCalculatingGoodIdCorrect_ShouldSuccess(
        GoodModel good, decimal expected)
    {
        // Arrange
        var deliveryCalculator = new DeliveryPriceCalculatorService
            (CreateUnitOptionsSnapshot(), CreateDelivriesRepositoryMock().Object);
        var goodsRepository = new Mock<IGoodsRepository>();
        goodsRepository.Setup(x => x.ContainsById(1)).Returns(true);
        goodsRepository.Setup(x => x.Get(1)).Returns(good);
        var cut = new GoodPriceCalculatorService(
            goodsRepository.Object, deliveryCalculator);

        // Act
        var result = cut.CalculatePrice(1);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GoodPriceCalculatorService_WhenCalculateAny_ShouldCallRepository()
    {
        // Arrange
        var deliveryCalculator = new DeliveryPriceCalculatorService
            (CreateUnitOptionsSnapshot(), CreateDelivriesRepositoryMock().Object);
        var goodsRepository = new Mock<IGoodsRepository>();
        goodsRepository.Setup(x => x.ContainsById(It.IsAny<int>())).Returns(true);
        goodsRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new GoodModel());
        var cut = new GoodPriceCalculatorService(
            goodsRepository.Object, deliveryCalculator);

        // Act
        var result = cut.CalculatePrice(1);

        // Assert
        goodsRepository.Verify(x => x.Get(1), Times.Once());
    }

    [Theory]
    [InlineData(1000, 1, 1)]
    [InlineData(1000, 5, 5)]
    [InlineData(500, 1, 0.5)]
    [InlineData(500, 4, 2)]
    public void GoodPriceCalculatorService_WhenCalculatePrice_ShouldCorrectUseDistance(
        int weight, int distance, decimal expected)
    {
        // Arrange
        var deliveryCalculator = new DeliveryPriceCalculatorService
            (CreateUnitOptionsSnapshot(), CreateDelivriesRepositoryMock().Object);
        var goodsRepository = new Mock<IGoodsRepository>();
        goodsRepository.Setup(x => x.ContainsById(1)).Returns(true);
        goodsRepository.Setup(x => x.Get(1)).Returns(
            new GoodModel(Weight: weight));
        var cut = new GoodPriceCalculatorService(
            goodsRepository.Object, deliveryCalculator);

        // Act
        var result = cut.CalculatePrice(1, distance);

        // Assert
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> CalcManyGoodsMemberData => ManyGoods();
    private static IEnumerable<object[]> ManyGoods()
    {
        yield return new object[]
        {
            new GoodModel (
                Height: 10,
                Length: 10,
                Width: 10,
                Weight: 0), 1
        };

        yield return new object[]
        {
            new GoodModel(
                Height: 1,
                Length: 1,
                Width: 1,
                Weight: 1000), 1
        };
    }
}
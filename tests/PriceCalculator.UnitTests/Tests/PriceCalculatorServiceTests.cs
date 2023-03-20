using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Route256.PriceCalculator.Domain.Interfaces.Repository;
using Route256.PriceCalculator.Domain.Models;
using Route256.PriceCalculator.Domain.Options;
using Route256.PriceCalculator.Domain.Services;
using Xunit;

namespace Workshop.UnitTests;

public class PriceCalculatorServiceTests
{
    [Fact]
    public void PriceCalculatorService_WhenGoodsEmptyArray_ShouldThrow()
    {
        // Arrange
        var options = new PriceCalculatorOptions();
        var repositoryMock = new Mock<IDeliveriesRepository>(MockBehavior.Default);
        var cut = new DeliveryPriceCalculatorService(
            CreateOptionsSnapshot(options), repositoryMock.Object);
        var goods = Array.Empty<GoodModel>();

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => cut.CalculatePrice(goods));
    }

    [Fact]
    public void PriceCalculatorService_WhenCalcAny_ShouldSave()
    {
        DeliveryModel deliveryModel = null;

        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = 1,
            WeightToPriceRatio = 1,
            DistanceToPriceRatio = 1
        };
        var repositoryMock = new Mock<IDeliveriesRepository>(MockBehavior.Strict);
        repositoryMock
            .Setup(x => x.Save(It.IsAny<DeliveryModel>()))
            .Callback<DeliveryModel>(x => deliveryModel = x);
        var cut = new DeliveryPriceCalculatorService(
            CreateOptionsSnapshot(options), repositoryMock.Object);
        var goods = new Fixture().CreateMany<GoodModel>().ToArray();

        // Act
        var result = cut.CalculatePrice(goods);

        // Assert
        Assert.NotNull(deliveryModel);
        repositoryMock.Verify(x => x.Save(It.IsAny<DeliveryModel>()));
        repositoryMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 10)]
    [InlineData(0, 0)]
    //[InlineData(-1, -1)]
    public void PriceCalculatorService_WhenCalcByVolume_ShouldSuccess(
        decimal volumeToPriceRatio,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = volumeToPriceRatio,
            DistanceToPriceRatio = 1
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new DeliveryPriceCalculatorService(
            CreateOptionsSnapshot(options), repositoryMock.Object);
        var good = new GoodModel(
            Height: 10,
            Length: 10,
            Width: 10,
            Weight: 0);

        // Act
        var result = cut.CalculatePrice(new[] { good });

        // Assert
        Assert.Equal(expected, result);
    }

    private static Mock<IDeliveriesRepository> CreateRepositoryMock()
    {
        var repositoryMock = new Mock<IDeliveriesRepository>(MockBehavior.Strict);
        repositoryMock.Setup(x => x.Save(It.IsAny<DeliveryModel>()));
        return repositoryMock;
    }

    private static IOptionsSnapshot<PriceCalculatorOptions> CreateOptionsSnapshot(
        PriceCalculatorOptions options)
    {
        var optionsMock = new Mock<IOptionsSnapshot<PriceCalculatorOptions>>(MockBehavior.Strict);

        optionsMock
            .Setup(x => x.Value)
            .Returns(() => options);

        return optionsMock.Object;
    }

    [Theory]
    [MemberData(nameof(CalcByVolumeManyMemberData))]
    public void PriceCalculatorService_WhenCalcByVolumeMany_ShouldSuccess(
        GoodModel[] goods,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = 1,
            DistanceToPriceRatio = 1
        };
        var repositoryMock = CreateRepositoryMock();
        var cut = new DeliveryPriceCalculatorService(
            CreateOptionsSnapshot(options), repositoryMock.Object);

        // Act
        var result = cut.CalculatePrice(goods);

        // Assert
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> CalcByVolumeManyMemberData => CalcByVolumeMany();
    private static IEnumerable<object[]> CalcByVolumeMany()
    {
        yield return new object[]
        {
            new GoodModel[] { new(
                Height: 10,
                Length: 10,
                Width: 10,
                Weight: 0), }, 1
        };

        yield return new object[]
        {
            Enumerable
                .Range(1, 2)
                .Select(x => new GoodModel(
                    Height: 10,
                    Length: 10,
                    Width: 10,
                    Weight: 0))
                .ToArray(),
            2
        };
    }
}
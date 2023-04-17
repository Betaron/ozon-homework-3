using System.Net.Http.Json;
using System.Text.Json;
using Route256.PriceCalculator.Api.Requests.V3;
using Route256.PriceCalculator.Api.Responses.V3;
using Xunit;

namespace PriceCalculator.IntegrationTests.Tests;

public class V3DeliveryPriceController
{
    [Fact]
    public async Task V3DeliveryPriceController_WhenGoodCalculate_ShouldCorrectResponce()
    {
        //Arrange
        var app = new AppFixture();
        var httpClient = app.CreateClient();

        var requestBody = new GoodCalculateRequest(1, 1);

        var httpContent = JsonContent.Create(requestBody);

        //Act
        var responce = await httpClient.PostAsync("good/calculate", httpContent);
        var content = await responce.Content.ReadAsStringAsync();
        var price = JsonSerializer.Deserialize<CalculateResponse>(
            content,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })?.Price;

        //Assert
        responce.EnsureSuccessStatusCode();
        Assert.Equal(7280489.64608m, price);
    }

    [Fact]
    public async Task V3DeliveryPriceController_WhenNotExistingGoodCalculate_ShouldFailure()
    {
        //Arrange
        var app = new AppFixture();
        var httpClient = app.CreateClient();

        var requestBody = new GoodCalculateRequest(-1, 1);

        var httpContent = JsonContent.Create(requestBody);

        //Act
        var response = await httpClient.PostAsync("good/calculate", httpContent);

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
    }
}

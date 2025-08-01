using Ambev.DeveloperEvaluation.Integration.Application;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Endpoints.Carts;

/// <summary>
/// Tests GetCart endpoint.
/// </summary>
public class GetCartTest : WebApiTestBase
{
    /// <summary>
    /// Test not found cart by id.
    /// </summary>
    [Fact(DisplayName = "Test not found cart by id.")]
    public async Task Given_Nothing_Carts_When_Try_Get_Car_Response_Should_Be_NotFound()
    {
        var cartId = Guid.Empty;
        var responseMessage = await HttpClient.GetAsync($"/api/carts/{cartId}");

        responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var response = await responseMessage.Content.ReadFromJsonAsync<ApiResponseWithData<CartResponse>>();
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.Message.Should().Be("NotEmptyValidator");
    }

    /// <summary>
    /// Test when found correctly cart by id.
    /// </summary>
    [Fact(DisplayName = "Test when found correctly cart by id.")]
    public async Task Given_Cart_When_Get_This_Car_Response_Should_Be_Not_Null()
    {
        var cartId = Guid.NewGuid();

        Seeder = seeder => seeder
            .NewCart(cartId);

        var response = await HttpClient.GetFromJsonAsync<ApiResponseWithData<CartResponse>>($"/api/carts/{cartId}");

        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be("Cart retrieved successfully");
        response.Data.Should().NotBeNull();
        response.Data.Id.Should().Be(cartId);
    }
}

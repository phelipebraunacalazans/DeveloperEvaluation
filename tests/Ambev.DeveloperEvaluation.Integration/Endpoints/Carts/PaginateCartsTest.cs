using Ambev.DeveloperEvaluation.Integration.Application;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts;
using FluentAssertions;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Endpoints.Carts;

/// <summary>
/// Tests GetCart endpoint.
/// </summary>
public class PaginateCartsTest : WebApiTestBase
{
    /// <summary>
    /// Test when does have carts and get list of them result should be empty.
    /// </summary>
    [Fact(DisplayName = "Test when does have carts and get list of them result should be empty.")]
    public async Task Given_Nothing_Carts_When_Try_Get_Car_Response_Should_Be_NotFound()
    {
        var response = await HttpClient.GetFromJsonAsync<PaginatedResponse<CartResponse>>("/api/carts");

        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data.Should().BeEmpty();
    }

    /// <summary>
    /// Given three carts when get page list result should be not empty.
    /// </summary>
    [Fact(DisplayName = "Given three carts when get page list result should be not empty.")]
    public async Task Given_Cart_When_Get_This_Car_Response_Should_Be_Not_Null()
    {
        Seeder = seeder => seeder
            .NewCart()
            .NewCart()
            .NewCart();

        var response = await HttpClient.GetFromJsonAsync<PaginatedResponse<CartResponse>>("/api/carts");

        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(3);
    }

    /// <summary>
    /// Given twenty three carts when navigate between pages with default page site result should be as expected.
    /// </summary>
    [Theory(DisplayName = "Given twenty three carts when navigate between pages with default page site result should be as expected.")]
    [InlineData(1, 10)]
    [InlineData(2, 10)]
    [InlineData(3, 3)]
    [InlineData(4, 0)]
    public async Task Given_23_Carts_When_Navegate_With_Default_Page_Size_Result_Should_Be_As_Expected(
        int page,
        int expectedPageSize)
    {
        Seeder = seeder => seeder
            .NewManyCarts(23);

        var response = await HttpClient.GetFromJsonAsync<PaginatedResponse<CartResponse>>($"/api/carts?_page={page}");

        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data.Should().HaveCount(expectedPageSize);
    }
}

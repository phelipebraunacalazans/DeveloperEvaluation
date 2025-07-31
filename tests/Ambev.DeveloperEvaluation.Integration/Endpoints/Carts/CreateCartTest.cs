using Ambev.DeveloperEvaluation.Integration.Application;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Http;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Endpoints.Carts;

/// <summary>
/// Tests for CreateCart endpoint.
/// </summary>
public class CreateCartTest : WebApiTestBase
{
    /// <summary>
    /// Test to response bad request response when logged user is suspended.
    /// </summary>
    [Fact(DisplayName = "Test to response bad request response when logged user is suspended.")]
    public async Task Given_Suspended_Logged_User_When_Try_Create_Cart_Response_Should_Be_BadRequest()
    {
        var userId = Guid.NewGuid();
        var productId1 = Guid.NewGuid();

        Factory.User.Id = userId;

        Seeder = seeder => seeder
            .NewCustomer(userId)
                .UserSuspended()
            .NewProduct(productId1);

        var body = new
        {
            UserId = userId,
            Date = DateTime.UtcNow,
            Branch = "Branching name",
            Products = new[]
            {
                new
                {
                    ProductId = productId1,
                    Quantity = 1,
                }
            }
        };

        var responseMessage = await HttpClient.PostAsJsonAsync("/api/carts", body);

        responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var response = await responseMessage.Content.ReadFromJsonAsync<ApiResponseWithData<CartResponse>>();
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.Message.Should().Be("User not found");
        response.Errors.Should().HaveCount(1);
        response.Errors.First().Should().BeEquivalentTo(new
        {
            Error = "User not found",
            Detail = $"The user with ID {userId} does not exist in our database",
        });
    }

    /// <summary>
    /// Test when does have carts and get list of them result should be empty.
    /// </summary>
    [Fact(DisplayName = "Test when does have carts and get list of them result should be empty.")] //, Skip = "Test quebrando por causa dos claims do usuário, vestigar melhor")]
    public async Task Given_Nothing_Carts_When_Try_Get_Cart_Response_Should_Be_NotFound()
    {
        var userId = Guid.NewGuid();
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();

        Factory.User.Id = userId;

        Seeder = seeder => seeder
            .NewCustomer(userId)
            .NewProduct(productId1)
            .NewProduct(productId2);

        var body = new
        {
            UserId = userId,
            Date = DateTime.UtcNow,
            Branch = "Branching name",
            Products = new[]
            {
                new
                {
                    ProductId = productId1,
                    Quantity = 1,
                },
                new
                {
                    ProductId = productId2,
                    Quantity = 10,
                },
            }
        };

        var responseMessage = await HttpClient.PostAsJsonAsync("/api/carts", body);
        await responseMessage.EnsureSuccessAsync();

        var response = await responseMessage.Content.ReadFromJsonAsync<ApiResponseWithData<CartResponse>>();
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        //response.Data.Id.Should().NotBeEmpty();

        // note: to guarantee if dbcontext is called once time.
        //var mockedDbContext = Factory.GetMock<DefaultContext>();
        //mockedDbContext.Verify(_ => _.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}

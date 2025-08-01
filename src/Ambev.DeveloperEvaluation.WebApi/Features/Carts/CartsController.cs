using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.PaginatedCart;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.PaginateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

/// <summary>
/// Controller for managing product operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="CartsController"/>.
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieve a paginated list of carts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of carts</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PaginateCarts(
        [FromQuery] PaginateCartsRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<PaginateCartsCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        var items = _mapper.Map<ICollection<CartResponse>>(response.Items);

        return OkPaginated<CartResponse>(
            new(items, response.TotalItems, request));
    }

    /// <summary>
    /// Creates a new cart.
    /// </summary>
    /// <param name="request">The cart creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCart(
        [FromBody] CreateCartRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new CreateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateCartCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Created(nameof(GetCart), new { result.Id }, _mapper.Map<CartResponse>(result), "Cart created successfully");
    }

    /// <summary>
    /// Retrieves a cart by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the cart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details if found</returns>
    [HttpGet("{id}", Name = nameof(GetCart))]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart(GetCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new GetCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetCartCommand>(request.Id);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<CartResponse>(result), "Cart retrieved successfully");
    }

    /// <summary>
    /// Edit a cart.
    /// </summary>
    /// <param name="request">The cart update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCart(
        Guid id,
        [FromBody] UpdateCartRequest request,
        CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest("The request id is different from the url");

        var validator = new UpdateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateCartCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<CartResponse>(response), "Cart updated successfully");
    }

    /// <summary>
    /// Deletes a cart by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the cart to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the cart was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCart(DeleteCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new DeleteCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<DeleteCartCommand>(request.Id);
        await _mediator.Send(command, cancellationToken);

        return Ok("Cart deleted successfully");
    }
}

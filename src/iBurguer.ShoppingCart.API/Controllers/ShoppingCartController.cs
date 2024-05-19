using iBurguer.ShoppingCart.Core.UseCases.AddItem;
using iBurguer.ShoppingCart.Core.UseCases.Checkout;
using iBurguer.ShoppingCart.Core.UseCases.ClearCart;
using iBurguer.ShoppingCart.Core.UseCases.CreateAnonymousCart;
using iBurguer.ShoppingCart.Core.UseCases.CreateCustomerCart;
using iBurguer.ShoppingCart.Core.UseCases.DecrementCartItem;
using iBurguer.ShoppingCart.Core.UseCases.GetCart;
using iBurguer.ShoppingCart.Core.UseCases.IncrementCartItem;
using iBurguer.ShoppingCart.Core.UseCases.RemoveCartItem;
using Microsoft.AspNetCore.Mvc;

namespace iBurguer.ShoppingCart.API.Controllers;

/// <summary>
/// API controller for managing shopping carts.
/// </summary>
[Route("api/carts")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    /// <summary>
    /// Get shopping cart by ID
    /// </summary>
    /// <remarks>Returns the shopping cart with the specified ID.</remarks>
    /// <param name="useCase">The use case responsible for retrieving a shopping cart.</param>
    /// <param name="id">The ID of the shopping cart to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <response code="200">Successful operation. Returns the shopping cart.</response>
    /// <response code="404">Item not found. The specified shopping cart ID does not exist.</response>
    /// <response code="500">Internal server error. Something went wrong on the server side.</response>
    /// <returns>Returns an HTTP response containing the retrieved shopping cart.</returns>
    [HttpGet("{shoppingCartId:guid}")]
    [ProducesResponseType(typeof(ShoppingCartResponse), 200)]
    public async Task<ActionResult> GetMenuItemById([FromServices] IGetCartUseCase useCase, Guid shoppingCartId, CancellationToken cancellationToken = default)
    {
        var response = await useCase.GetCartById(shoppingCartId, cancellationToken);

        if (response is not null)
        {
            return Ok(response);
        }

        return NotFound();
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CreateShoppingCartResponse), 201)]
    public async Task<ActionResult> CreateCustomerShoppingCart([FromServices] ICreateCustomerShoppingCartUseCase useCase, [FromBody] CreateShoppingCartRequest request, CancellationToken cancellationToken = default)
    {
        var response = await useCase.CreateCustomerShoppingCart(request, cancellationToken);

        return Created("Shopping Cart created successfully", response);
    }

    /// <summary>
    /// Creates an anonymous shopping cart.
    /// </summary>
    /// <remarks>Creates a new shopping cart for anonymous users.</remarks>
    /// <param name="useCase">The use case responsible for creating the shopping cart.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <response code="201">Shopping cart created successfully.</response>
    /// <response code="422">Invalid request. Missing or invalid parameters.</response>
    /// <response code="500">Internal server error. Something went wrong on the server side.</response>
    /// <returns>Returns an HTTP response containing the created shopping cart.</returns>
    [HttpPost]
    [Route("anonymous")]
    [ProducesResponseType(typeof(CreateAnonymousShoppingCartResponse), 201)]
    public async Task<ActionResult> CreateAnonymousShoppingCart([FromServices] ICreateAnonymousShoppingCartUseCase useCase, CancellationToken cancellationToken = default)
    {
        var response = await useCase.CreateAnonymousShoppingCart(cancellationToken);

        return Created("Shopping Cart created successfully", response);
    }

    [HttpPost]
    [Route("{shoppingCartId:guid}/items")]
    public async Task<IActionResult> AddCartItemToShoppingCart([FromServices] IAddItemToShoppingCartUseCase useCase, Guid shoppingCartId, [FromBody]AddItemToShoppingCartRequest request, CancellationToken cancellationToken = default)
    {
        var response = await useCase.AddItemToShoppingCart(request, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpDelete]
    [Route("{shoppingCartId:guid}/items")]
    public async Task<IActionResult> ClearShoppingCart([FromServices] IClearShoppingCartUseCase useCase, Guid shoppingCartId, CancellationToken cancellationToken = default)
    {
        await useCase.ClearShoppingCart(shoppingCartId, cancellationToken);
        
        return NoContent();
    }

    [HttpDelete]
    [Route("{shoppingCartId}/items/{cartItemId}")]
    public async Task<IActionResult> RemoveCartItemFromShoppingCart([FromServices] IRemoveCartItemFromShoppingCartUseCase useCase, Guid shoppingCartId, Guid cartItemId, CancellationToken cancellationToken = default)
    {
        await useCase.RemoveCartItemFromShoppingCart(shoppingCartId, cartItemId, cancellationToken);
        return Ok();
    }

    [HttpPatch]
    [Route("{shoppingCartId:guid}/items/{cartItemId:guid}/incremented")]
    public async Task<IActionResult> IncrementTheQuantityOfTheCartItem([FromServices] IIncrementTheQuantityOfTheCartItemUseCase useCase, Guid shoppingCartId, Guid cartItemId, [FromBody]IncrementTheQuantityOfTheCartItemRequest request, CancellationToken cancellationToken = default)
    {
        if (shoppingCartId != request.ShoppingCartId || cartItemId != request.CartItemId)
        {
            return BadRequest("Os Ids do carrinho e item do carrinho precisam ser iguais aos informados no body da requisição");
        }

        await useCase.IncrementTheQuantityOfTheCartItem(request, cancellationToken);
        return Ok();
    }

    [HttpPatch]
    [Route("{shoppingCartId:guid}/item/{cartItemId:guid}/decremented")]
    public async Task<IActionResult> DecrementTheQuantityOfTheCartItem([FromServices] IDecrementTheQuantityOfTheCartItemUseCase useCase, Guid shoppingCartId, Guid cartItemId, [FromBody]DecrementTheQuantityOfTheCartItemRequest request, CancellationToken cancellationToken = default)
    {
        if (shoppingCartId != request.ShoppingCartId || cartItemId != request.CartItemId)
        {
            return BadRequest("Os Ids do carrinho e item do carrinho precisam ser iguais aos informados no body da requisição");
        }

        await useCase.DecrementTheQuantityOfTheCartItem(request, cancellationToken);
        
        return Ok();
    }
    
    [HttpPatch]
    [Route("{shoppingCartId:guid}/checkout")]
    [ProducesResponseType(typeof(CheckoutResponse), 201)]
    public async Task<IActionResult> Checkout([FromServices] ICheckoutUseCase useCase, Guid shoppingCartId, [FromBody]CheckoutRequest request, CancellationToken cancellationToken = default)
    {
        var response = await useCase.Checkout(request, cancellationToken);
        
        return Created("Order created successfully", response);
    }
}
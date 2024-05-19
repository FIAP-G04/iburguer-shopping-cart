using iBurguer.ShoppingCart.API.Controllers;
using iBurguer.ShoppingCart.Core.Domain;
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
using Moq;

namespace iBurguer.ShoppingCart.UnitTests.API
{
    public class ShoppingCartControllerTest
    {
        private readonly Mock<IGetCartUseCase> _getCartUseCaseMock;
        private readonly Mock<ICreateAnonymousShoppingCartUseCase> _createAnonymousShoppingCartUseCaseMock;
        private readonly Mock<ICreateCustomerShoppingCartUseCase> _createCustomerShoppingCartUseCaseMock;
        private readonly Mock<IAddItemToShoppingCartUseCase> _addItemToShoppingCartUseCaseMock;
        private readonly Mock<IClearShoppingCartUseCase> _clearShoppingCartUseCaseMock;
        private readonly Mock<IRemoveCartItemFromShoppingCartUseCase> _removeCartItemFromShoppingCartUseCaseMock;
        private readonly Mock<IIncrementTheQuantityOfTheCartItemUseCase> _incrementTheQuantityOfTheCartItemUseCaseMock;
        private readonly Mock<IDecrementTheQuantityOfTheCartItemUseCase> _decrementTheQuantityOfTheCartItemUseCaseMock;
        private readonly Mock<ICheckoutUseCase> _checkoutUseCaseMock;
        private readonly ShoppingCartController _controller;

        public ShoppingCartControllerTest()
        {
            _getCartUseCaseMock = new Mock<IGetCartUseCase>();
            _createCustomerShoppingCartUseCaseMock = new Mock<ICreateCustomerShoppingCartUseCase>();
            _createAnonymousShoppingCartUseCaseMock = new Mock<ICreateAnonymousShoppingCartUseCase>();
            _addItemToShoppingCartUseCaseMock = new Mock<IAddItemToShoppingCartUseCase>();
            _clearShoppingCartUseCaseMock = new Mock<IClearShoppingCartUseCase>();
            _removeCartItemFromShoppingCartUseCaseMock = new Mock<IRemoveCartItemFromShoppingCartUseCase>();
            _incrementTheQuantityOfTheCartItemUseCaseMock = new Mock<IIncrementTheQuantityOfTheCartItemUseCase>();
            _decrementTheQuantityOfTheCartItemUseCaseMock = new Mock<IDecrementTheQuantityOfTheCartItemUseCase>();
            _checkoutUseCaseMock = new Mock<ICheckoutUseCase>();

            _controller = new ShoppingCartController();
        }

        // You can write test methods for each action in the controller
        // Here is an example for testing the GetCartItemById action
        [Fact]
        public async Task GetCartItemById_ShouldReturnOk_WhenShoppingCartExists()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var shoppingCartResponse = new ShoppingCartResponse(); // create the expected response
            _getCartUseCaseMock.Setup(useCase => useCase.GetCartById(shoppingCartId, CancellationToken.None))
                .ReturnsAsync(shoppingCartResponse);

            // Act
            var result = await _controller.GetMenuItemById(_getCartUseCaseMock.Object, shoppingCartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(shoppingCartResponse, okResult.Value);
        }

        // Test method for CreateAnonymousShoppingCart with request body
        [Fact]
        public async Task CreateAnonymousShoppingCart_WithRequestBody_ShouldReturnCreated()
        {
            // Arrange
            var request = new CreateShoppingCartRequest(Guid.NewGuid());
            var response = new CreateShoppingCartResponse(Guid.NewGuid(), Guid.NewGuid());
            _createCustomerShoppingCartUseCaseMock.Setup(useCase => useCase.CreateCustomerShoppingCart(request, CancellationToken.None))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CreateCustomerShoppingCart(_createCustomerShoppingCartUseCaseMock.Object, request);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal("Shopping Cart created successfully", createdResult.Location);
            Assert.Equal(response, createdResult.Value);
        }

        // Test method for CreateAnonymousShoppingCart without request body
        [Fact]
        public async Task CreateAnonymousShoppingCart_WithoutRequestBody_ShouldReturnCreated()
        {
            // Arrange
            var response = new CreateAnonymousShoppingCartResponse(Guid.NewGuid());
            _createAnonymousShoppingCartUseCaseMock.Setup(useCase => useCase.CreateAnonymousShoppingCart(CancellationToken.None))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CreateAnonymousShoppingCart(_createAnonymousShoppingCartUseCaseMock.Object);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal("Shopping Cart created successfully", createdResult.Location);
            Assert.Equal(response, createdResult.Value);
        }

        // Test method for AddCartItemToShoppingCart
        [Fact]
        public async Task AddCartItemToShoppingCart_ShouldReturnOk()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var shoppingCartId = cart.Id;
            var request = new AddItemToShoppingCartRequest();
            var response = AddItemToShoppingCartResponse.Convert(cart, Guid.NewGuid());
            _addItemToShoppingCartUseCaseMock.Setup(useCase => useCase.AddItemToShoppingCart(request, CancellationToken.None))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.AddCartItemToShoppingCart(_addItemToShoppingCartUseCaseMock.Object, shoppingCartId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }

        // Test method for ClearShoppingCart
        [Fact]
        public async Task ClearShoppingCart_ShouldReturnNoContent()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();

            // Act
            var result = await _controller.ClearShoppingCart(_clearShoppingCartUseCaseMock.Object, shoppingCartId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test method for RemoveCartItemFromShoppingCart
        [Fact]
        public async Task RemoveCartItemFromShoppingCart_ShouldReturnOk()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();

            // Act
            var result = await _controller.RemoveCartItemFromShoppingCart(_removeCartItemFromShoppingCartUseCaseMock.Object, shoppingCartId, cartItemId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        // Test method for IncrementTheQuantityOfTheCartItem
        [Fact]
        public async Task IncrementTheQuantityOfTheCartItem_ShouldReturnOk()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();
            var request = new IncrementTheQuantityOfTheCartItemRequest(shoppingCartId, cartItemId, 1);

            // Act
            var result = await _controller.IncrementTheQuantityOfTheCartItem(_incrementTheQuantityOfTheCartItemUseCaseMock.Object, shoppingCartId, cartItemId, request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        // Test method for DecrementTheQuantityOfTheCartItem
        [Fact]
        public async Task DecrementTheQuantityOfTheCartItem_ShouldReturnOk()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var cartItemId = Guid.NewGuid();
            var request = new DecrementTheQuantityOfTheCartItemRequest(shoppingCartId, cartItemId, 1);

            // Act
            var result = await _controller.DecrementTheQuantityOfTheCartItem(_decrementTheQuantityOfTheCartItemUseCaseMock.Object, shoppingCartId, cartItemId, request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        // Test method for Checkout
        [Fact]
        public async Task Checkout_ShouldReturnCreated()
        {
            // Arrange
            var shoppingCartId = Guid.NewGuid();
            var request = new CheckoutRequest();
            var response = new CheckoutResponse();
            _checkoutUseCaseMock.Setup(useCase => useCase.Checkout(request, CancellationToken.None))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Checkout(_checkoutUseCaseMock.Object, shoppingCartId, request);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal("Order created successfully", createdResult.Location);
            Assert.Equal(response, createdResult.Value);
        }
    }
}

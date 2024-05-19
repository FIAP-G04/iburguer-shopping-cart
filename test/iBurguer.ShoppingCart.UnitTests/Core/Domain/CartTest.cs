using iBurguer.ShoppingCart.Core.Domain;

namespace iBurguer.ShoppingCart.UnitTests.Core.Domain
{
    public class CartTest
    {
        [Fact]
        public void CreateAnonymousShoppingCart_ShouldReturnNewCart()
        {
            // Act
            var cart = Cart.CreateAnonymousShoppingCart();

            // Assert
            Assert.NotNull(cart);
            Assert.Null(cart.CustomerId);
            Assert.False(cart.Closed);
            Assert.True(cart.CreatedAt <= DateTime.Now);
        }

        [Fact]
        public void CreateCustomerShoppingCart_ShouldReturnNewCart()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            // Act
            var cart = Cart.CreateCustomerShoppingCart(customerId);

            // Assert
            Assert.NotNull(cart);
            Assert.Equal(customerId, cart.CustomerId);
            Assert.False(cart.Closed);
            Assert.True(cart.CreatedAt <= DateTime.Now);
        }

        [Fact]
        public void AddCartItem_ShouldAddNewItemToCart()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var product = new Product(Guid.NewGuid(),"tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(1);

            // Act
            var cartItem = cart.AddCartItem(product, quantity);

            // Assert
            Assert.Single(cart.Items);
            Assert.Equal(product.ProductId, cartItem.Product.ProductId);
            Assert.Equal(quantity.Value, cartItem.Quantity.Value);
        }

        [Fact]
        public void RemoveCartItem_ShouldRemoveItemFromCart()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var product = new Product(Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(1);
            var cartItem = cart.AddCartItem(product, quantity);

            // Act
            cart.RemoveCartItem(cartItem.CartItemId);

            // Assert
            Assert.Empty(cart.Items);
        }

        [Fact]
        public void Clear_ShouldRemoveAllItemsFromCart()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var product = new Product (Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(1);
            cart.AddCartItem(product, quantity);

            // Act
            cart.Clear();

            // Assert
            Assert.Empty(cart.Items);
        }

        [Fact]
        public void Close_ShouldMarkCartAsClosed()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var product = new Product (Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(1);
            cart.AddCartItem(product, quantity);

            // Act
            cart.Close();

            // Assert
            Assert.True(cart.Closed);
        }

        [Fact]
        public void Total_ShouldReturnSumOfCartItemSubtotals()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var product1 = new Product (Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var product2 = new Product (Guid.NewGuid(), "tst", ProductType.Dessert, new Price(20));
            var quantity = new Quantity(1);
            cart.AddCartItem(product1, quantity);
            cart.AddCartItem(product2, quantity);

            // Act
            var total = cart.Total;

            // Assert
            Assert.Equal(new Price(30), total);
        }

        [Fact]
        public void IncrementTheQuantityOfTheCartItem_ShouldIncreaseQuantity()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var product = new Product (Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(1);
            var cartItem = cart.AddCartItem(product, quantity);

            // Act
            cart.IncrementTheQuantityOfTheCartItem(cartItem.CartItemId, new Quantity(2));

            // Assert
            Assert.Equal(3, cart.GetCartItemById(cartItem.CartItemId).Quantity.Value);
        }

        [Fact]
        public void DecrementTheQuantityOfTheCartItem_ShouldDecreaseQuantity()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var product = new Product (Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(3);
            var cartItem = cart.AddCartItem(product, quantity);

            // Act
            cart.DecrementTheQuantityOfTheCartItem(cartItem.CartItemId, new Quantity(2));

            // Assert
            Assert.Equal(1, cart.GetCartItemById(cartItem.CartItemId).Quantity.Value);
        }

        [Fact]
        public void UpdateItemPriceThroughProduct_ShouldUpdateItemPrice()
        {
            // Arrange
            var cart = Cart.CreateAnonymousShoppingCart();
            var product = new Product (Guid.NewGuid(), "tst", ProductType.Dessert, new Price(10));
            var quantity = new Quantity(1);
            var cartItem = cart.AddCartItem(product, quantity);
            var newPrice = new Price(20);

            // Act
            cart.UpdateItemPriceThroughProduct(product.ProductId, newPrice);

            // Assert
            Assert.Equal(newPrice, cartItem.Product.Price);
        }
    }
}

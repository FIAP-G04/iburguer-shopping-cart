using iBurguer.ShoppingCart.Core.Domain;
using Newtonsoft.Json;

namespace iBurguer.ShoppingCart.UnitTests.Core.Domain
{
    public class ProductTest
    {
        [Fact]
        public void Product_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productName = "Test Product";
            var productType = ProductType.MainDish;
            var price = new Price(10.5m);

            // Act
            var product = new Product(productId, productName, productType, price);

            // Assert
            Assert.Equal(productId, product.ProductId);
            Assert.Equal(productName, product.Name);
            Assert.Equal(productType, product.Type);
            Assert.Equal(price, product.Price);
        }

        [Fact]
        public void Product_UpdatePrice_ShouldUpdatePrice()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productName = "Test Product";
            var productType = ProductType.MainDish;
            var initialPrice = new Price(10.5m);
            var newPrice = new Price(20.0m);
            var product = new Product(productId, productName, productType, initialPrice);

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(newPrice, product.Price);
        }
    }
}

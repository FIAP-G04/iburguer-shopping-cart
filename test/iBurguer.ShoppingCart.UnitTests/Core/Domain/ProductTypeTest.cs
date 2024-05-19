using iBurguer.ShoppingCart.Core.Domain;
using static iBurguer.ShoppingCart.Core.Exceptions;

namespace iBurguer.ShoppingCart.UnitTests.Core.Domain
{
    public class ProductTypeTest
    {
        [Fact]
        public void ProductType_MainDish_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var mainDish = ProductType.MainDish;

            // Assert
            Assert.Equal(1, mainDish.Id());
            Assert.Equal("MainDish", mainDish.Name);
        }

        [Fact]
        public void ProductType_SideDish_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var sideDish = ProductType.SideDish;

            // Assert
            Assert.Equal(2, sideDish.Id());
            Assert.Equal("SideDish", sideDish.Name);
        }

        [Fact]
        public void ProductType_FromName_ShouldReturnCorrectProductType()
        {
            // Act
            var productType = ProductType.FromName("Drink");

            // Assert
            Assert.Equal(ProductType.Drink, productType);
        }

        [Fact]
        public void ProductType_FromId_ShouldReturnCorrectProductType()
        {
            // Act
            var productType = ProductType.FromId(4);

            // Assert
            Assert.Equal(ProductType.Dessert, productType);
        }

        [Fact]
        public void ProductType_FromName_ShouldThrowInvalidProductTypeException_ForInvalidName()
        {
            // Act & Assert
            Assert.Throws<InvalidProductTypeException>(() => ProductType.FromName("InvalidName"));
        }

        [Fact]
        public void ProductType_FromId_ShouldThrowInvalidProductTypeException_ForInvalidId()
        {
            // Act & Assert
            Assert.Throws<InvalidProductTypeException>(() => ProductType.FromId(999));
        }

        [Fact]
        public void ProductType_ImplicitConversion_FromInt_ShouldReturnCorrectProductType()
        {
            // Act
            ProductType productType = 1;

            // Assert
            Assert.Equal(ProductType.MainDish, productType);
        }

        [Fact]
        public void ProductType_ImplicitConversion_FromInt_ShouldThrowInvalidProductTypeException_ForInvalidId()
        {
            // Act & Assert
            Assert.Throws<InvalidProductTypeException>(() => { ProductType productType = 999; });
        }

        [Fact]
        public void ProductType_ToString_ShouldReturnCorrectName()
        {
            // Arrange
            var productType = ProductType.Dessert;

            // Act
            var name = productType.ToString();

            // Assert
            Assert.Equal("Dessert", name);
        }
    }
}

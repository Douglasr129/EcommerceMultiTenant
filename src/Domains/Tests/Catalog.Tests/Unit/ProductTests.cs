using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Tests.Unit
{
#pragma warning disable CS8625 // Não é possível converter um literal nulo em um tipo de referência não anulável.
    public class ProductTests
    {
        // Dados válidos para testes
        private readonly string _validName = "Produto Teste";
        private readonly Money _validPrice = new Money(100.00m);
        private readonly int _validStock = 10;
        private readonly Guid _validCategoryId = Guid.NewGuid();

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidData_ShouldCreateProduct()
        {
            // Act
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);

            // Assert
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.Equal(_validName, product.Name);
            Assert.Equal(_validPrice, product.Price);
            Assert.Equal(_validStock, product.Stock);
            Assert.Equal(_validCategoryId, product.CategoryId);
        }

        [Fact]
        public void Constructor_WithNullCategoryId_ShouldCreateProduct()
        {
            // Act
            var product = new Product(_validName, _validPrice, _validStock, null);

            // Assert
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.Null(product.CategoryId);
        }

        [Fact]
        public void Constructor_WithZeroStock_ShouldCreateProduct()
        {
            // Act
            var product = new Product(_validName, _validPrice, 0, _validCategoryId);

            // Assert
            Assert.Equal(0, product.Stock);
        }

        [Theory]
#pragma warning disable xUnit1012 // Null should only be used for nullable parameters
        [InlineData(null)]
#pragma warning restore xUnit1012 // Null should only be used for nullable parameters
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithInvalidName_ShouldThrowDomainException(string invalidName)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
                new Product(invalidName, _validPrice, _validStock, _validCategoryId));

            Assert.Equal("Nome do produto inválido", exception.Message);
        }

        [Fact]
        public void Constructor_WithNullPrice_ShouldThrowDomainException()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
                new Product(_validName, null, _validStock, _validCategoryId));

            Assert.Equal("Preço inválido", exception.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void Constructor_WithNegativeStock_ShouldThrowDomainException(int negativeStock)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
                new Product(_validName, _validPrice, negativeStock, _validCategoryId));

            Assert.Equal("Estoque não pode ser negativo", exception.Message);
        }

        [Fact]
        public void Constructor_ShouldGenerateNewGuid()
        {
            // Act
            var product1 = new Product(_validName, _validPrice, _validStock, _validCategoryId);
            var product2 = new Product(_validName, _validPrice, _validStock, _validCategoryId);

            // Assert
            Assert.NotEqual(product1.Id, product2.Id);
        }

        #endregion

        #region UpdatePrice Tests

        [Fact]
        public void UpdatePrice_WithValidPrice_ShouldUpdatePrice()
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);
            var newPrice = new Money(150.00m);

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(newPrice, product.Price);
        }

        [Fact]
        public void UpdatePrice_WithNullPrice_ShouldThrowDomainException()
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
                product.UpdatePrice(null));

            Assert.Equal("Preço inválido", exception.Message);
        }

        [Fact]
        public void UpdatePrice_ShouldNotChangeOtherProperties()
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);
            var originalId = product.Id;
            var originalName = product.Name;
            var originalStock = product.Stock;
            var originalCategoryId = product.CategoryId;
            var newPrice = new Money(200.00m);

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(originalId, product.Id);
            Assert.Equal(originalName, product.Name);
            Assert.Equal(originalStock, product.Stock);
            Assert.Equal(originalCategoryId, product.CategoryId);
        }

        #endregion

        #region UpdateStock Tests

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public void UpdateStock_WithPositiveQuantity_ShouldIncreaseStock(int quantity)
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);
            var expectedStock = _validStock + quantity;

            // Act
            product.UpdateStock(quantity);

            // Assert
            Assert.Equal(expectedStock, product.Stock);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(-10)]
        public void UpdateStock_WithNegativeQuantityWithinLimit_ShouldDecreaseStock(int quantity)
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);
            var expectedStock = _validStock + quantity;

            // Act
            product.UpdateStock(quantity);

            // Assert
            Assert.Equal(expectedStock, product.Stock);
        }

        [Fact]
        public void UpdateStock_WithZeroQuantity_ShouldNotChangeStock()
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);
            var originalStock = product.Stock;

            // Act
            product.UpdateStock(0);

            // Assert
            Assert.Equal(originalStock, product.Stock);
        }

        [Theory]
        [InlineData(-11)]
        [InlineData(-20)]
        [InlineData(-100)]
        public void UpdateStock_WithNegativeQuantityExceedingStock_ShouldThrowDomainException(int quantity)
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
                product.UpdateStock(quantity));

            Assert.Equal("Estoque insuficiente", exception.Message);
        }

        [Fact]
        public void UpdateStock_ToExactlyZero_ShouldSetStockToZero()
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);

            // Act
            product.UpdateStock(-_validStock);

            // Assert
            Assert.Equal(0, product.Stock);
        }

        [Fact]
        public void UpdateStock_MultipleOperations_ShouldAccumulateCorrectly()
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);

            // Act
            product.UpdateStock(5);   // 10 + 5 = 15
            product.UpdateStock(-3);  // 15 - 3 = 12
            product.UpdateStock(8);   // 12 + 8 = 20

            // Assert
            Assert.Equal(20, product.Stock);
        }

        [Fact]
        public void UpdateStock_ShouldNotChangeOtherProperties()
        {
            // Arrange
            var product = new Product(_validName, _validPrice, _validStock, _validCategoryId);
            var originalId = product.Id;
            var originalName = product.Name;
            var originalPrice = product.Price;
            var originalCategoryId = product.CategoryId;

            // Act
            product.UpdateStock(5);

            // Assert
            Assert.Equal(originalId, product.Id);
            Assert.Equal(originalName, product.Name);
            Assert.Equal(originalPrice, product.Price);
            Assert.Equal(originalCategoryId, product.CategoryId);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Product_WithMaxIntStock_ShouldWork()
        {
            // Act
            var product = new Product(_validName, _validPrice, int.MaxValue, _validCategoryId);

            // Assert
            Assert.Equal(int.MaxValue, product.Stock);
        }

        [Fact]
        public void UpdateStock_FromZero_WithNegativeValue_ShouldThrowDomainException()
        {
            // Arrange
            var product = new Product(_validName, _validPrice, 0, _validCategoryId);

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() =>
                product.UpdateStock(-1));

            Assert.Equal("Estoque insuficiente", exception.Message);
        }

        #endregion
#pragma warning restore CS8625 // Não é possível converter um literal nulo em um tipo de referência não anulável.

    }
}

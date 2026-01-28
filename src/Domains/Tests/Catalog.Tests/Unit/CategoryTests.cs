using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Tests.Unit
{
    public class CategoryTests
    {
#pragma warning disable CS8625 // Não é possível converter um literal nulo em um tipo de referência não anulável.
        // Dados válidos para testes
        private readonly string _validName = "Eletrônicos";

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidName_ShouldCreateCategory()
        {
            // Act
            var category = new Category(_validName);

            // Assert
            Assert.NotEqual(Guid.Empty, category.Id);
            Assert.Equal(_validName, category.Name);
        }

        [Fact]
        public void Constructor_ShouldGenerateNewGuid()
        {
            // Act
            var category1 = new Category(_validName);
            var category2 = new Category(_validName);

            // Assert
            Assert.NotEqual(category1.Id, category2.Id);
        }

        [Fact]
        public void Constructor_WithNullName_ShouldThrowArgumentException()
        {
            // Act & Assert

            var exception = Assert.Throws<ArgumentException>(() =>
                new Category(null));


            Assert.Equal("Nome da categoria inválido", exception.Message);
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldThrowArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Category(string.Empty));

            Assert.Equal("Nome da categoria inválido", exception.Message);
        }

        [Theory]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("  \t  ")]
        public void Constructor_WithWhitespaceName_ShouldThrowArgumentException(string whitespaceName)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Category(whitespaceName));

            Assert.Equal("Nome da categoria inválido", exception.Message);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("Eletrônicos")]
        [InlineData("Roupas e Acessórios")]
        [InlineData("Categoria com Nome Muito Grande para Testar Limites")]
        public void Constructor_WithDifferentValidNames_ShouldCreateCategory(string validName)
        {
            // Act
            var category = new Category(validName);

            // Assert
            Assert.Equal(validName, category.Name);
            Assert.NotEqual(Guid.Empty, category.Id);
        }

        [Fact]
        public void Constructor_WithNameContainingSpecialCharacters_ShouldCreateCategory()
        {
            // Arrange
            var nameWithSpecialChars = "Eletrônicos & Informática - 100%";

            // Act
            var category = new Category(nameWithSpecialChars);

            // Assert
            Assert.Equal(nameWithSpecialChars, category.Name);
        }

        [Fact]
        public void Constructor_WithNameContainingNumbers_ShouldCreateCategory()
        {
            // Arrange
            var nameWithNumbers = "Categoria 123";

            // Act
            var category = new Category(nameWithNumbers);

            // Assert
            Assert.Equal(nameWithNumbers, category.Name);
        }

        #endregion

        #region UpdateName Tests

        [Fact]
        public void UpdateName_WithValidName_ShouldUpdateName()
        {
            // Arrange
            var category = new Category(_validName);
            var newName = "Informática";

            // Act
            category.UpdateName(newName);

            // Assert
            Assert.Equal(newName, category.Name);
        }

        [Fact]
        public void UpdateName_WithNullName_ShouldThrowArgumentException()
        {
            // Arrange
            var category = new Category(_validName);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                category.UpdateName(null));

            Assert.Equal("Nome da categoria inválido", exception.Message);
        }

        [Fact]
        public void UpdateName_WithEmptyName_ShouldThrowArgumentException()
        {
            // Arrange
            var category = new Category(_validName);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                category.UpdateName(string.Empty));

            Assert.Equal("Nome da categoria inválido", exception.Message);
        }

        [Theory]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("  \t  ")]
        public void UpdateName_WithWhitespaceName_ShouldThrowArgumentException(string whitespaceName)
        {
            // Arrange
            var category = new Category(_validName);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                category.UpdateName(whitespaceName));

            Assert.Equal("Nome da categoria inválido", exception.Message);
        }

        [Fact]
        public void UpdateName_ShouldNotChangeId()
        {
            // Arrange
            var category = new Category(_validName);
            var originalId = category.Id;
            var newName = "Nova Categoria";

            // Act
            category.UpdateName(newName);

            // Assert
            Assert.Equal(originalId, category.Id);
        }

        [Fact]
        public void UpdateName_MultipleTimes_ShouldKeepLastValue()
        {
            // Arrange
            var category = new Category(_validName);
            var name1 = "Primeira Atualização";
            var name2 = "Segunda Atualização";
            var name3 = "Terceira Atualização";

            // Act
            category.UpdateName(name1);
            category.UpdateName(name2);
            category.UpdateName(name3);

            // Assert
            Assert.Equal(name3, category.Name);
        }

        [Fact]
        public void UpdateName_WithSameName_ShouldUpdateSuccessfully()
        {
            // Arrange
            var category = new Category(_validName);

            // Act
            category.UpdateName(_validName);

            // Assert
            Assert.Equal(_validName, category.Name);
        }

        [Theory]
        [InlineData("Livros")]
        [InlineData("Móveis & Decoração")]
        [InlineData("Categoria 2024")]
        public void UpdateName_WithDifferentValidNames_ShouldUpdateCorrectly(string newName)
        {
            // Arrange
            var category = new Category(_validName);

            // Act
            category.UpdateName(newName);

            // Assert
            Assert.Equal(newName, category.Name);
        }

        #endregion

        #region Edge Cases and Integration Tests

        [Fact]
        public void Category_AfterFailedUpdate_ShouldKeepOriginalName()
        {
            // Arrange
            var category = new Category(_validName);
            var originalName = category.Name;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => category.UpdateName(null));
            Assert.Equal(originalName, category.Name);
        }

        [Fact]
        public void Category_WithMaxLengthName_ShouldCreateAndUpdate()
        {
            // Arrange
            var maxLengthName = new string('A', 255);

            // Act
            var category = new Category(maxLengthName);
            var newMaxLengthName = new string('B', 255);
            category.UpdateName(newMaxLengthName);

            // Assert
            Assert.Equal(newMaxLengthName, category.Name);
        }

        [Fact]
        public void Category_WithUnicodeCharacters_ShouldWork()
        {
            // Arrange
            var unicodeName = "Categoría 日本語 العربية 中文";

            // Act
            var category = new Category(unicodeName);

            // Assert
            Assert.Equal(unicodeName, category.Name);
        }

        [Fact]
        public void Category_NameWithLeadingAndTrailingSpaces_ShouldNotTrim()
        {
            // Arrange
            var nameWithSpaces = " Categoria ";

            // Act
            var category = new Category(nameWithSpaces);

            // Assert
            // Nota: O código atual não faz trim, então o nome mantém os espaços
            Assert.Equal(nameWithSpaces, category.Name);
        }

        [Fact]
        public void Category_IdProperty_ShouldBeReadOnly()
        {
            // Arrange & Act
            var category = new Category(_validName);
            var id = category.Id;

            // Assert
            Assert.NotEqual(Guid.Empty, id);
            // Verifica que Id tem apenas getter público
            var idProperty = typeof(Category).GetProperty("Id");
            Assert.NotNull(idProperty);
            Assert.True(idProperty.CanRead);
            Assert.False(idProperty.SetMethod?.IsPublic ?? false);
        }

        [Fact]
        public void Category_NameProperty_ShouldBeReadOnly()
        {
            // Arrange & Act
            var category = new Category(_validName);

            // Assert
            // Verifica que Name tem apenas getter público
            var nameProperty = typeof(Category).GetProperty("Name");
            Assert.NotNull(nameProperty);
            Assert.True(nameProperty.CanRead);
            Assert.False(nameProperty.SetMethod?.IsPublic ?? false);
        }

        #endregion
#pragma warning restore CS8625 // Não é possível converter um literal nulo em um tipo de referência não anulável.
    }
}

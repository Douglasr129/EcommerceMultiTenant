using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    [Column("id", TypeName = "uuid")]
    public Guid Id { get; private set; }

    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; private set; }
    public Money Price { get; private set; }

    [Column("stock", TypeName = "integer")]
    [Range(0, int.MaxValue, ErrorMessage = "Estoque deve ser um número inteiro não negativo.")]
    public int Stock { get; private set; }

    [Column("category_id", TypeName = "uuid")]
    public Guid? CategoryId { get; private set; }
    // Construtor sem parâmetros (necessário para o EF)
    public Product()
    {
        // Inicializa propriedades com valores padrão
        Id = Guid.NewGuid();
        Name = string.Empty;
        Price = new Money(0, "BRL");
        Stock = 0;
        CategoryId = null;
    }


    // Construtor único - sempre valida
    public Product(string name, Money price, int stock, Guid? categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do produto inválido");

        if (stock < 0)
            throw new DomainException("Estoque não pode ser negativo");
        Id = Guid.NewGuid();
        Name = name;
        Price = price ?? throw new DomainException("Preço inválido");
        Stock = stock;
        CategoryId = categoryId;
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice == null)
            throw new DomainException("Preço inválido");

        Price = newPrice;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0 && Stock + quantity < 0)
            throw new DomainException("Estoque insuficiente");

        Stock += quantity;
    }
}
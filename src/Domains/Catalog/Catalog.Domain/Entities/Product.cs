using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("product")]
public class Product
{
    [Key]
    [Column("id", TypeName = "uuid")]
    public Guid Id { get; private set; }

    [Required(ErrorMessage = "Nome do produto é obrigatório")]
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; private set; }

    [Required(ErrorMessage = "O valor do produto é obrigatório")]
    [Column("price", TypeName = "numeric(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public Money Price { get; private set; }

    [Column("stock", TypeName = "integer")]
    [Range(0, int.MaxValue, ErrorMessage = "Estoque deve ser um número inteiro não negativo.")]
    public int Stock { get; private set; }

    [Column("category_id", TypeName = "uuid")]
    public Guid? CategoryId { get; private set; }

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
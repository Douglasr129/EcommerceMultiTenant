namespace Catalog.Application.Commands
{
    public class CreateProductCommand
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; } // será convertido para Money
        public required int StockInicial { get; set; }
        public Guid? CategoryId { get; set; }
    }

}

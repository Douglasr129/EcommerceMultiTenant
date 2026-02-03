namespace Catalog.Api.Models
{
    public class ProductResponse : ResourceBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal PriceAmount { get; set; }
        public string PriceCurrency { get; set; }
        public int Stock { get; set; }
        // Adicione outros campos conforme necessário

        public static ProductResponse FromProduct(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                PriceAmount = product.Price.Amount,
                PriceCurrency = product.Price.Currency,
                Stock = product.Stock
            };
        }
    }
}

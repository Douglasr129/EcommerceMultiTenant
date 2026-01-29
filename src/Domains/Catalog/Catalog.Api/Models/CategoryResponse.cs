using Catalog.Domain.Entities;

namespace Catalog.Api.Models
{
    public class CategoryResponse : ResourceBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static CategoryResponse FromCategory(Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}

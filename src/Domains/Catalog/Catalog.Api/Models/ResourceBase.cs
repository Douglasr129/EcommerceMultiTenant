namespace Catalog.Api.Models
{
    public abstract class ResourceBase
    {
        public List<Link> Links { get; set; } = new List<Link>();
    }
}

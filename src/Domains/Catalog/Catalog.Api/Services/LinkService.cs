using Catalog.Api.Models;

namespace Catalog.Api.Services
{
    public interface ILinkService
    {
        List<Link> GenerateProductLinks(Guid productId);
        List<Link> GenerateProductsLinks();
    }

    public class LinkService : ILinkService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public LinkService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
        }

        public List<Link> GenerateProductLinks(Guid productId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var links = new List<Link>();

            // Self link
            var selfLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "GetProductById",
                controller: "Products",
                values: new { id = productId }
            );
            links.Add(new Link(selfLink!, "self", "GET"));

            // Update link
            var updateLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "UpdateProduct",
                controller: "Products",
                values: new { id = productId }
            );
            links.Add(new Link(updateLink!, "update", "PUT"));

            // Delete link
            var deleteLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "DeleteProduct",
                controller: "Products",
                values: new { id = productId }
            );
            links.Add(new Link(deleteLink!, "delete", "DELETE"));

            // All products link
            var productsAllLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "GetProductAll",
                controller: "Products"
            );
            links.Add(new Link(productsAllLink!, "all-products", "GET"));

            return links;
        }

        public List<Link> GenerateProductsLinks()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var links = new List<Link>();

            // Self link
            var selfLink = _linkGenerator.GetUriByAction(
                httpContext,
                action: "GetAllProducts",
                controller: "Products"
            );
            links.Add(new Link(selfLink, "self", "GET"));

            // Create link
            var createLink = _linkGenerator.GetUriByAction(
                httpContext,
                action: "AddProduct",
                controller: "Products"
            );
            links.Add(new Link(createLink, "create", "POST"));

            return links;
        }
    }
}

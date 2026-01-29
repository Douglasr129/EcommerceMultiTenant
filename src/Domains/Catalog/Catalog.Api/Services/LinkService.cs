using Catalog.Api.Models;
using Microsoft.AspNetCore.Http;

namespace Catalog.Api.Services
{
    public interface ILinkService
    {
        List<Link> GenerateProductLinks(Guid productId);
        List<Link> GenerateProductByCategoryIdLinks(Guid productId);
        List<Link> GenerateProductsLinks();

        List<Link> GenerateCategoryLinks(Guid productId);
        List<Link> GenerateCategorysLinks();
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

            

            return links;
        }
        public List<Link> GenerateProductByCategoryIdLinks(Guid CategoryId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var links = new List<Link>();
            // All products link
            var productsAllLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "GetProductsByCategory",
                controller: "Products",
                 values: new { id = CategoryId }
            );
            links.Add(new Link(productsAllLink!, "all-products-by-category", "GET"));

            return links;
        }

        public List<Link> GenerateProductsLinks()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var links = new List<Link>();

            // Self link
            var selfLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "GetAllProducts",
                controller: "Products"
            );
            links.Add(new Link(selfLink!, "self", "GET"));

            // Create link
            var createLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "AddProduct",
                controller: "Products"
            );
            links.Add(new Link(createLink!, "create", "a"));

            return links;
        }



        public List<Link> GenerateCategoryLinks(Guid categoryId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var links = new List<Link>();

            // Self link
            var selfLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "GetCategoryById",
                controller: "Categories",
                values: new { id = categoryId }
            );
            links.Add(new Link(selfLink!, "self", "GET"));

            // Update link
            var updateLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "UpdateCategory",
                controller: "Categories",
                values: new { id = categoryId }
            );
            links.Add(new Link(updateLink!, "update", "PUT"));

            // Delete link
            var deleteLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "DeleteCategory",
                controller: "Categories",
                values: new { id = categoryId }
            );
            links.Add(new Link(deleteLink!, "delete", "DELETE"));

            return links;
        }

        public List<Link> GenerateCategorysLinks()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var links = new List<Link>();

            // Self link
            var selfLink = _linkGenerator.GetUriByAction(
                httpContext!,
                action: "GetCategoryAll",
                controller: "Products"
            );
            links.Add(new Link(selfLink!, "self", "GET"));

            return links;
        }


    }
}

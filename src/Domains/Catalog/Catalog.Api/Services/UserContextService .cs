using System.Security.Claims;

namespace Catalog.Api.Services
{
    public interface IUserContextService
    {
        string GetUserId();
        string GetUserEmail();
        string GetUserRole();
    }
    public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

#pragma warning disable CS8603 // Possível retorno de referência nula.
        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string GetUserRole()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        }
#pragma warning restore CS8603 // Possível retorno de referência nula.
    }
}

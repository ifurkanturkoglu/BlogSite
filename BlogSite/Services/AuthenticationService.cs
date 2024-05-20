namespace BlogSite.Services
{
    public class AuthenticationService
    {
        IHttpContextAccessor httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
        }

        public bool UserIsAuthenticate()
        {
            return httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}

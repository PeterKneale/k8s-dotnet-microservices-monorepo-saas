using System.Linq;
using BuildingBlocks.Infrastructure.AccountContext;
using Microsoft.AspNetCore.Http;

namespace Management.Infrastructure
{
    /// <summary>
    /// Account context will be retrieved from the currently logged in user
    /// </summary>
    public class AccountContextGetter : IAccountContextGetter
    {
        private readonly IHttpContextAccessor _context;
        private readonly string AccountId = "AccountId";

        public AccountContextGetter(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string? GetAccountId()
        {
            var httpContext = _context.HttpContext;
            if (httpContext == null)
            {
                return null;
            }
            var claim = httpContext.User.Claims.SingleOrDefault(x => x.Type == AccountId);
            if (claim == null)
            {
                return null;
            }
            return claim.Value;
        }
    }
}
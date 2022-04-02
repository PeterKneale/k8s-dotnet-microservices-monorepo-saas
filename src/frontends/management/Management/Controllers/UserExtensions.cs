using System.Linq;
using System.Security.Claims;

namespace Management.Controllers
{
    public static class UserExtensions
    {
        public static string CurrentAccountId(this ClaimsPrincipal principal) => principal.Claims.Single(x => x.Type == "AccountId").Value;
        public static string CurrentStoreId(this ClaimsPrincipal principal) => principal.Claims.Single(x => x.Type == "StoreId").Value;
    }
}
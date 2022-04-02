using System;
using BuildingBlocks.Infrastructure.AccountContext;
using Microsoft.AspNetCore.Http;

namespace Shopping.Infrastructure
{
    /// <summary>
    /// Account context will be retrieved from the http context
    /// </summary>
    public class AccountContextGetter : IAccountContextGetter
    {
        private readonly HttpContext _context;

        public AccountContextGetter(IHttpContextAccessor context)
        {
            _context = context.HttpContext ?? throw new ArgumentNullException(nameof(context));
        }

        public string? GetAccountId() => 
            _context.GetAccountId();
    }
}
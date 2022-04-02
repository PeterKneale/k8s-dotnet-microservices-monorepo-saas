using System;
using BuildingBlocks.Infrastructure.AccountContext;
using Microsoft.AspNetCore.Http;

namespace Shopping.Infrastructure
{
    public class AccountContextSetter : IAccountContextSetter
    {
        private readonly HttpContext _context;

        public AccountContextSetter(IHttpContextAccessor context)
        {
            _context = context.HttpContext ?? throw new ArgumentNullException(nameof(context));
        }

        public void SetAccountId(string accountId) => 
            _context.SetAccountId(accountId);
    }
}
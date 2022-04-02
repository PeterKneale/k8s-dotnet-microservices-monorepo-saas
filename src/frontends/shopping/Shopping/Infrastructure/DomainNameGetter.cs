using System;
using Microsoft.AspNetCore.Http;

namespace Shopping.Infrastructure
{
    public interface IDomainNameGetter
    {
        string GetDomainName();
    }
    
    public class DomainNameGetter : IDomainNameGetter
    {
        private readonly HttpContext _context;

        public DomainNameGetter(IHttpContextAccessor context)
        {
            _context = context.HttpContext ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetDomainName() => "example3.ecommerce-store-builder.dev";// _context.Request.Host.Value;
    }
}
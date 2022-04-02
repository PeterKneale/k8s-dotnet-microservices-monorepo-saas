using System;
using Microsoft.AspNetCore.Http;

namespace Shopping.Infrastructure
{
    public interface IStoreContextSetter
    {
        void SetStoreContext(StoreContext context);
    }

    public class StoreContextSetter : IStoreContextSetter
    {
        private readonly HttpContext _context;

        public StoreContextSetter(IHttpContextAccessor context)
        {
            _context = context.HttpContext ?? throw new ArgumentNullException(nameof(context));
        }

        public void SetStoreContext(StoreContext context) => 
            _context.SetStoreContext(context);
    }
}
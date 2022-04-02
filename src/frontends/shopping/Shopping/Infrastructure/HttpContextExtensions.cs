using System;
using Microsoft.AspNetCore.Http;

namespace Shopping.Infrastructure
{
    public static class HttpContextExtensions
    {
        private const string StoreContextKey = nameof(StoreContext);
        private const string AccountIdKey = nameof(AccountIdKey);

        public static void SetStoreContext(this HttpContext httpContext, StoreContext storeContext) =>
            httpContext.Items.Add(StoreContextKey, storeContext);

        public static StoreContext GetShoppingContext(this HttpContext httpContext) =>
            httpContext.Items[StoreContextKey] as StoreContext ?? throw new Exception("Shopping context not available");

        public static string? GetAccountId(this HttpContext httpContext) =>
            httpContext.Items[AccountIdKey]?.ToString();
        
        public static void SetAccountId(this HttpContext httpContext, string accountId) =>
            httpContext.Items.Add(AccountIdKey, accountId);
    }
}
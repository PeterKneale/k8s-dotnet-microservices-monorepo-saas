using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace BuildingBlocks.Infrastructure.AccountContext.Interceptors
{
    /// <summary>
    /// Gets the account context from a call and makes it the ambient account context
    /// </summary>
    public class AccountContextAwareServerInterceptor : Interceptor
    {
        private readonly IAccountContextSetter _context;

        public AccountContextAwareServerInterceptor(IAccountContextSetter context)
        {
            _context = context;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var accountId = context.RequestHeaders.GetValue(MetaDataConstants.AccountId);
            if (accountId != null)
            {
                _context.SetAccountId(accountId);    
            }
            return await continuation(request, context);
        }
    }
}
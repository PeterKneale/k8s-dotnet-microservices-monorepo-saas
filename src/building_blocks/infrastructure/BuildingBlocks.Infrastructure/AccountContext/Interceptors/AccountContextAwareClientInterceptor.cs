using Grpc.Core;
using Grpc.Core.Interceptors;

namespace BuildingBlocks.Infrastructure.AccountContext.Interceptors
{
    /// <summary>
    /// Gets the ambient account context and adds it to an outgoing call
    /// </summary>
    public class AccountContextAwareClientInterceptor : Interceptor
    {
        private readonly IAccountContextGetter _context;

        public AccountContextAwareClientInterceptor(IAccountContextGetter context)
        {
            _context = context;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var meta = GetMetadata();
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, context.Options.WithHeaders(meta));
            return base.AsyncUnaryCall(request, context, continuation);
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var meta = GetMetadata();
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, context.Options.WithHeaders(meta));
            return base.BlockingUnaryCall(request, context, continuation);
        }

        private Metadata GetMetadata()
        {
            var meta = new Metadata();
            var accountId = _context.GetAccountId();
            if (accountId != null)
            {
                meta.Add(new Metadata.Entry(MetaDataConstants.AccountId, accountId));
            }
            return meta;
        }
    }

}
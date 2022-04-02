using System;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Domain.DDD.Rules;
using BuildingBlocks.Infrastructure.AccountContext;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.ExceptionHandling
{
    public class GrpcExceptionInterceptor : Interceptor
    {
        private readonly ILogger<GrpcExceptionInterceptor> _logger;

        public GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (BusinessRuleBrokenException e)
            {
                _logger.LogWarning(e, "Business rule broken when calling {Method}", context.Method);
                // is there a more appropriate status
                throw new RpcException(new Status(StatusCode.FailedPrecondition, e.Message));
            }
            catch (AccountContextMissingException e)
            {
                _logger.LogWarning(e, "Account context missing when calling {Method}", context.Method);
                // is there a more appropriate status
                throw new RpcException(new Status(StatusCode.PermissionDenied, e.Message));
            }
            catch (NotFoundException e)
            {
                _logger.LogWarning(e, "Not Found when calling {Method}", context.Method);
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }
            catch (ValidationFailedException e)
            {
                _logger.LogWarning(e, "Validation Failed when calling {Method}", context.Method);
                throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unknown error occurred when calling {Method}", context.Method);
                throw new RpcException(Status.DefaultCancelled, e.Message);
            }
        }
    }
}
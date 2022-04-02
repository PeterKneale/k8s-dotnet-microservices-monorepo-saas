using System;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Shopping.Application;

namespace Shopping.Api
{
    public class ShoppingApiService : ShoppingApi.ShoppingApiBase
    {
        private readonly IMediator _mediator;

        public ShoppingApiService(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public override async Task<Empty> AddProductToShoppingCart(AddProductToShoppingCartRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddProductToShoppingCart.Command(request.CartId, request.ProductId, request.Quantity));
            return new Empty();
        }

        public override async Task<ShoppingCart> GetShoppingCart(GetShoppingCartRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new GetShoppingCart.Query(request.CartId));
        }
        
        public override async Task<Store> GetStore(GetStoreRequest request, ServerCallContext context)
        {
            return await _mediator.Send(new GetStore.Query(request.Domain));
        }
        
        public override async Task<SearchProductsResults> SearchProducts(SearchProductsRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }
    }
    
    public partial class SearchProductsRequest : IRequest<SearchProductsResults>{}
}

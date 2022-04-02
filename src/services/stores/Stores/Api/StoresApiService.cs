using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Stores.Application;

namespace Stores.Api
{
    public class StoresApiService : StoresApi.StoresApiBase
    {
        private readonly IMediator _mediator;

        public StoresApiService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Empty> AddStore(AddStoreRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddStore.Command(request.StoreId, request.Name));
            return new Empty();
        }
        
        public override async Task<Empty> SetTheme(SetThemeRequest request, ServerCallContext context)
        {
            await _mediator.Send(new SetTheme.Command(request.StoreId, request.Theme));
            return new Empty();
        }

        public override async Task<Empty> SetSubdomain(SetSubdomainRequest request, ServerCallContext context)
        {
            await _mediator.Send(new SetSubdomain.Command(request.StoreId, request.Subdomain));
            return new Empty();
        }
        
        public override async Task<Empty> SetDomain(SetDomainRequest request, ServerCallContext context)
        {
            await _mediator.Send(new SetDomain.Command(request.StoreId, request.Domain));
            return new Empty();
        }

        public override async Task<Store> GetStore(GetStoreRequest request, ServerCallContext context)
        {
            return Map(await _mediator.Send(new GetStore.Query(request.StoreId)));
        }
        
        public override async Task<StoresReply> ListStores(ListStoresRequest request, ServerCallContext context)
        {
            var stores = await _mediator.Send(new ListStores.Query());
            return new StoresReply
            {
                Stores = {stores.Select(Map)}
            };
        }

        private static Store Map(Domain.Store store) => new Store
        {
            StoreId = store.StoreId,
            Name = store.Name,
            Theme = store.Theme,
            Subdomain = store.Subdomain,
            Domain = store.Domain ?? string.Empty
        };
    }
}
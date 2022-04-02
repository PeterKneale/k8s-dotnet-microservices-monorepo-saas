using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using FluentValidation;
using MediatR;
using Stores.Domain;

namespace Stores.Application
{
    public static class GetStoreBySubdomain
    {
        public record Query(string Subdomain) : IRequest<Store>;
        
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Subdomain).NotEmpty().MaximumLength(36);
            }
        }
        
        public class Handler : IRequestHandler<Query, Store>
        {
            private readonly IStoreRepository _repository;

            public Handler(IStoreRepository repository)
            {
                _repository = repository;
            }

            public async Task<Store> Handle(Query request, CancellationToken cancellationToken)
            {
                var store = await _repository.GetBySubdomainAsync(request.Subdomain);
                if (store == null)
                {
                    throw new NotFoundException(nameof(Store), request.Subdomain);
                }
                return store;
            }
        }
    }
}
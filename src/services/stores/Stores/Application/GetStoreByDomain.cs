using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Stores.Domain;

namespace Stores.Application
{
    public static class GetStoreByDomain
    {
        public record Query(string Domain) : IRequest<Store>;
        
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Domain).NotEmpty().MaximumLength(36);
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
                var store = await _repository.GetByDomainAsync(request.Domain);
                if (store == null)
                {
                    throw new NotFoundException(nameof(Store), request.Domain);
                }
                return store;
            }
        }
    }
}
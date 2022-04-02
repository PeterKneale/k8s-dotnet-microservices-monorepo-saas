using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Stores.Domain;

namespace Stores.Application
{
    public static class ListStores
    {
        public record Query() : IRequest<IEnumerable<Store>>;
        
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
            }
        }
        
        public class Handler : IRequestHandler<Query, IEnumerable<Store>>
        {
            private readonly IStoreRepository _repository;

            public Handler(IStoreRepository repository)
            {
                _repository = repository;
            }

            public async Task<IEnumerable<Store>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repository.ListAsync();
            }
        }
    }

}
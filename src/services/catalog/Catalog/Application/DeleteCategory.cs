using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext;
using Catalog.Domain;
using Catalog.Messages;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Catalog.Application
{
    public static class DeleteCategory
    {
        public record Command(string CategoryId) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CategoryId).NotEmpty().MaximumLength(36);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ICategoryRepository _repository;
            private readonly IPublishEndpoint _publisher;
            private readonly IAccountContextGetter _context;

            public Handler(ICategoryRepository repository, IPublishEndpoint publisher, IAccountContextGetter context)
            {
                _repository = repository;
                _publisher = publisher;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var category = await _repository.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    return Unit.Value;
                }
                
                await _repository.DeleteAsync(category);
                await _publisher.Publish(new CategoryDeleted(_context.GetAccountId(), category.CategoryId, category.Name), cancellationToken);
                
                return Unit.Value;
            }
        }
    }
}
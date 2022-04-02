using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Infrastructure.AccountContext;
using Catalog.Domain;
using Catalog.Messages;
using FluentValidation;
using MassTransit;
using MediatR;

namespace Catalog.Application
{
    public static class UpdateCategory
    {
        public record Command(string CategoryId, string Name) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CategoryId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
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
                var categoryId = request.CategoryId;
                var name = request.Name;

                var category = await _repository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    throw new NotFoundException(nameof(Category), categoryId);
                }

                category.ChangeName(name);
                await _repository.UpdateAsync(category);
                
                var message = new CategoryUpdated(_context.GetAccountId(), request.CategoryId, request.Name);
                await _publisher.Publish(message, cancellationToken);

                return Unit.Value;
            }
        }
    }

}
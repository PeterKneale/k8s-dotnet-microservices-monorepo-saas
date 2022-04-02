using System.Threading.Tasks;
using Catalog.Messages;
using MassTransit;
using Search.Application.Services;

namespace Search.Infrastructure.Consumers
{
    public class ProductDeletedConsumer : IConsumer<ProductDeleted>
    {
        private readonly IIndexService _index;

        public ProductDeletedConsumer(IIndexService index)
        {
            _index = index;
        }

        public async Task Consume(ConsumeContext<ProductDeleted> context) =>
            await _index.RemoveProduct(
                context.Message.AccountId,
                context.Message.ProductId,
                context.CancellationToken);
    }
}
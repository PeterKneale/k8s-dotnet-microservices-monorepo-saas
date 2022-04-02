using System.Threading;
using System.Threading.Tasks;
using Catalog.Messages;
using MassTransit;
using Search.Application.Services;

namespace Search.Infrastructure.Consumers
{
    public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
    {
        private readonly IIndexService _index;

        public ProductUpdatedConsumer(IIndexService index)
        {
            _index = index;
        }

        public async Task Consume(ConsumeContext<ProductUpdated> context) =>
            await _index.UpdateProduct(new ProductDocument
                {
                    AccountId = context.Message.AccountId,
                    ProductId = context.Message.ProductId,
                    Name = context.Message.Name,
                    Description = context.Message.Description,
                    CategoryId = context.Message.CategoryId,
                    CategoryName = context.Message.CategoryName,
                    CategoryIdPath = context.Message.CategoryIdPath,
                    CategoryNamePath = context.Message.CategoryNamePath
                },
                CancellationToken.None);
    }
}
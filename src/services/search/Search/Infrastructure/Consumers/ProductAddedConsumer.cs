using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Catalog.Messages;
using Search.Application.Services;

namespace Search.Infrastructure.Consumers
{
    public class ProductAddedConsumer : IConsumer<ProductAdded>
    {
        private readonly IIndexService _index;

        public ProductAddedConsumer(IIndexService index)
        {
            _index = index;
        }

        public async Task Consume(ConsumeContext<ProductAdded> context) =>
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
using System.Threading.Tasks;
using Catalog.Messages;
using MassTransit;
using Shopping.Application.Data;
using Shopping.Infrastructure.DataSources;

namespace Shopping.Infrastructure.Consumers
{
    public class ProductAddedConsumer : IConsumer<ProductAdded>
    {
        private readonly IDataWriter _writer;

        public ProductAddedConsumer(IDataWriter writer)
        {
            _writer = writer;
        }

        public async Task Consume(ConsumeContext<ProductAdded> context)
        {
            await _writer.SaveAsync(new ProductData
            {
                AccountId = context.Message.AccountId,
                ProductId = context.Message.ProductId,
                Name = context.Message.Name,
                Description = context.Message.Description
            });
        }
    }
}
using System.Threading.Tasks;
using MassTransit;
using Shopping.Application.Data;
using Shopping.Infrastructure.DataSources;
using Stores.Messages;

namespace Shopping.Infrastructure.Consumers
{
    public class StoreAddedConsumer : IConsumer<StoreAdded>
    {
        private readonly IDataWriter _writer;

        public StoreAddedConsumer(IDataWriter writer)
        {
            _writer = writer;
        }

        public async Task Consume(ConsumeContext<StoreAdded> context)
        {
            await _writer.SaveAsync(new StoreData
            {
                AccountId = context.Message.AccountId,
                StoreId = context.Message.StoreId,
                Name = context.Message.Name,
                Theme = context.Message.Theme,
                Subdomain = context.Message.Subdomain
            });
        }
    }
}
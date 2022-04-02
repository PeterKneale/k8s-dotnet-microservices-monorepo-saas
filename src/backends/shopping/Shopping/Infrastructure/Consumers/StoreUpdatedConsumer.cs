using System.Threading.Tasks;
using MassTransit;
using Shopping.Application.Data;
using Shopping.Infrastructure.DataSources;
using Stores.Messages;

namespace Shopping.Infrastructure.Consumers
{
    public class StoreUpdatedConsumer : IConsumer<StoreUpdated>
    {
        private readonly IDataWriter _writer;

        public StoreUpdatedConsumer(IDataWriter writer)
        {
            _writer = writer;
        }

        public async Task Consume(ConsumeContext<StoreUpdated> context)
        {
            await _writer.SaveAsync(new StoreData
            {
                AccountId = context.Message.AccountId,
                StoreId = context.Message.StoreId,
                Name = context.Message.Name,
                Theme = context.Message.Theme,
                Subdomain = context.Message.Subdomain,
                Domain = context.Message.Domain
            });
        }
    }
}
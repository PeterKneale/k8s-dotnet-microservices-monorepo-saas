using Marten;
using Marten.Services;

namespace Accounts.Infrastructure.Repository
{
    public static class MartenSerializer
    {
        public static ISerializer GetSerializer()
        {
            var serializer = new JsonNetSerializer();

            serializer.Customize(_ =>
            {
                _.ContractResolver = new MartenPrivateSettersContractResolver();
            });

            return serializer;
        }
    }
}
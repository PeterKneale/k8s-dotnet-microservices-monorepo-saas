using System;
using System.Linq;
using System.Linq.Expressions;
using Baseline;
using Marten.Linq;
using Shopping.Application.Data;

namespace Shopping.Infrastructure.DataSources.Queries
{
    public class GetStoreBySubdomainQuery : ICompiledQuery<StoreData, StoreData?>
    {
        public string Subdomain { get; set; }

        public Expression<Func<IMartenQueryable<StoreData>, StoreData?>> QueryIs()
        {
            return q => q.SingleOrDefault(x => x.Subdomain.EqualsIgnoreCase(Subdomain));
        }
    }
}
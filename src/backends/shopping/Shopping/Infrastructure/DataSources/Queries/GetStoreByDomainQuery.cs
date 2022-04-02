using System;
using System.Linq;
using System.Linq.Expressions;
using Baseline;
using Marten.Linq;
using Shopping.Application.Data;

namespace Shopping.Infrastructure.DataSources.Queries
{
    public class GetStoreByDomainQuery : ICompiledQuery<StoreData, StoreData?>
    {
        public string Domain { get; set; }

        public Expression<Func<IMartenQueryable<StoreData>, StoreData?>> QueryIs()
        {
            return q => q.SingleOrDefault(x => x.Domain != null && x.Domain.EqualsIgnoreCase(Domain));
        }
    }
}
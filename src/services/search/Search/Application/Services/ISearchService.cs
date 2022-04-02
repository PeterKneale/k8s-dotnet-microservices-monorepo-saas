using System.Collections.Generic;
using System.Threading.Tasks;

namespace Search.Application.Services
{
    public interface ISearchService
    {
        Task<ProductDocuments> Search(string term, string? categoryIdPath = null, int pageOffset = 0, int pageSize = 10);
    }
}
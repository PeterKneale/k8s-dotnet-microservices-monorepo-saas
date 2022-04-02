using System.Collections.Generic;

namespace Search.Application.Services
{
    public class ProductDocuments
    {
        public ProductDocuments(IEnumerable<ProductDocument> documents, long total)
        {
            Total = total;
            Documents = documents;
        }
        
        public long Total { get; }
        
        public IEnumerable<ProductDocument> Documents { get; }
    }
}
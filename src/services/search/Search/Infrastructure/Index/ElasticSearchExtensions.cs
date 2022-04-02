using System;
using Nest;

namespace Search.Infrastructure.Index
{
    public static class ElasticSearchExtensions
    {
        public static ResponseBase AssertSuccess(this ResponseBase response)
        {
            if (!response.IsValid)
            {
                throw new Exception("elastic search error", response.OriginalException);
            }
            return response;
        }
    }
}
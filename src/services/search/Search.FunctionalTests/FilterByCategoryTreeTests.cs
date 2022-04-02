// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using BuildingBlocks.Infrastructure.AccountContext;
// using FluentAssertions;
// using Microsoft.Extensions.DependencyInjection;
// using Search.Application.Services;
// using Xunit;
// using Xunit.Abstractions;
//
// namespace Search.FunctionalTests
// {
//     // TODO: Needs to be rewritten as a component test
//     
//     [Collection(nameof(Fixture))]
//     public class FilterByCategoryTreeTests
//     {
//         private readonly ITestOutputHelper _output;
//         private readonly IIndexService _index;
//         private readonly ISearchService _search;
//         private readonly IAccountContextGetter _context;
//
//         public FilterByCategoryTreeTests(Fixture api, ITestOutputHelper output)
//         {
//             _output = output;
//             api.OutputHelper = output;
//             var scope = api.Services.CreateScope();
//             _context = scope.ServiceProvider.GetRequiredService<IAccountContextGetter>();
//             _index = scope.ServiceProvider.GetRequiredService<IIndexService>();
//             _search = scope.ServiceProvider.GetRequiredService<ISearchService>();
//         }
//
//         [Theory]
//         [InlineData("", "1", "sony MDR-Z1R,bose qc35,bose qc45")]
//         [InlineData("", "1,10", "sony MDR-Z1R,bose qc35,bose qc45")]
//         [InlineData("", "1,10,100", "sony MDR-Z1R")]
//         [InlineData("", "1,10,200", "bose qc35,bose qc45")]
//         [InlineData("bose", "1", "bose qc35,bose qc45")]
//         [InlineData("sony", "1", "sony MDR-Z1R")]
//         [InlineData("bose", "1,10", "bose qc35,bose qc45")]
//         [InlineData("bose", "1,10,200", "bose qc35,bose qc45")]
//         [InlineData("bose", "1,10,100", "")]
//         [InlineData("sony", "1,10,200", "")]
//         [InlineData("sony", "1,10,100", "sony MDR-Z1R")]
//         public async Task Can_filter_by_category_tree(string query, string categoryIdPath, string expectedProductIds)
//         {
//             /* Categories -> Products
//              *  1. audio
//              *      1.10. headphones
//              *          1.10.100 wired       sony MDR-Z1R  
//              *          1.10.200 wireless    bose qc35, bose 45
//              */
//             await Arrange();
//
//             await Retry.RetryAsync.ExecuteAsync(async () => {
//                 // Act
//                 var response = await _search.Search(query, categoryIdPath, 0, 100);
//                 _output.WriteLine($"Searched for '{query}' in {categoryIdPath}");
//                 foreach (var result in response.Documents)
//                 {
//                     _output.WriteLine($"Found {result.ProductId} in {result.CategoryNamePath}");
//                 }
//
//                 // Assert
//                 var matches = string.Join(",", response.Documents.Select(x => x.ProductId));
//                 matches.Should().BeEquivalentTo(expectedProductIds);
//             });
//         }
//
//         private async Task Arrange()
//         {
//             var accountId = Guid.NewGuid().ToString();
//             _context.SetAccountId(accountId);
//
//             await _index.UpdateProduct(new ProductDocument
//             {
//                 AccountId = accountId,
//                 ProductId = "sony MDR-Z1R",
//                 Name = "sony MDR-Z1R",
//                 Description = "sony MDR-Z1R",
//                 CategoryId = "100",
//                 CategoryName = "Wired",
//                 CategoryIdPath = "1,10,100",
//                 CategoryNamePath = "Audio,Headphones,Wired"
//             });
//
//             await _index.UpdateProduct(new ProductDocument
//             {
//                 AccountId = accountId,
//                 ProductId = "bose qc35",
//                 Name = "bose qc35",
//                 Description = "bose qc35",
//                 CategoryId = "200",
//                 CategoryName = "Wireless",
//                 CategoryIdPath = "1,10,200",
//                 CategoryNamePath = "Audio,Headphones,Wireless"
//             });
//
//             await _index.UpdateProduct(new ProductDocument
//             {
//                 AccountId = accountId,
//                 ProductId = "bose qc45",
//                 Name = "bose qc45",
//                 Description = "bose qc45",
//                 CategoryId = "200",
//                 CategoryName = "Wireless",
//                 CategoryIdPath = "1,10,200",
//                 CategoryNamePath = "Audio,Headphones,Wireless"
//             });
//         }
//     }
// }
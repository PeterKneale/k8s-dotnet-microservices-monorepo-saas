namespace Search.Application.Services
{
    public class ProductDocument
    {
        public string AccountId { get; set; }

        public string ProductId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }
        
        public string CategoryIdPath { get; set; }

        public string CategoryNamePath { get; set; }

        public override string ToString() => $" {CategoryNamePath} -> {Name} (Account {AccountId})";
    }

}
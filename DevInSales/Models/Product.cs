using System.Diagnostics.CodeAnalysis;

namespace DevInSales.Models
{
    [ExcludeFromCodeCoverage]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Suggested_Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

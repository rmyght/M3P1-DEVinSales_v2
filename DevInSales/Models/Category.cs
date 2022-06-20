using DevInSales.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace DevInSales.Models
{
    [ExcludeFromCodeCoverage]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<Product> Product { get; set; }

        public Category()
        {
        }

        public Category(string name, string slug)
        {
            Name = name;
            Slug = name.ToSlug();
        }
        public Category(int id, string name, string slug)
        {
            Id = id;
            Name = name;
            Slug = slug;
        }
    }
}

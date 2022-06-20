using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevInSales.Models
{
    [ExcludeFromCodeCoverage]
    public class ShippingCompany
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevInSales.Models
{
    [ExcludeFromCodeCoverage]
    public class State

    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
    }
}

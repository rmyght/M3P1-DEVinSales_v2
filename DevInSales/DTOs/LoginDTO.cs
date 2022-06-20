using DevInSales.Models;
using System.Diagnostics.CodeAnalysis;

namespace DevInSales.DTOs
{
    [ExcludeFromCodeCoverage]
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

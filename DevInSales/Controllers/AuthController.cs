using AutoMapper;
using DevInSales.Context;
using DevInSales.DTOs;
using DevInSales.Models;
using DevInSales.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevInSales.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SqlContext _context;

        public AuthController(SqlContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult ApiLogin([FromBody] LoginDTO dto)
        {
            var userLogin = _context.User.FirstOrDefault(l => l.Email == dto.Email && l.Password == dto.Password);

            if (userLogin == null)
                return NotFound("Usuário não existe ou a senha está incorreta!");

            var token = TokenServices.GenerateToken(userLogin);
            return Ok(token);
        }
    }
}

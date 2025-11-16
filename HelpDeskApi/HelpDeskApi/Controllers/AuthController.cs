using HelpDeskApi.Data;
using HelpDeskApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace HelpDeskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] UsuarioRegistroDTO usuarioDTO)
        {
            if (_context.Usuarios.Any(u => u.Email == usuarioDTO.Email))
                return BadRequest("E-mail já registrado.");

            var novoUsuario = new Usuario
            {
                Name = usuarioDTO.Name,
                Email = usuarioDTO.Email,
                senha = usuarioDTO.senha,
                Role = usuarioDTO.Role
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Usuário registrado com sucesso!",
                id = novoUsuario.Id // Retorna o ID gerado
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Usuario login)
        {
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == login.Email && u.senha == login.senha);

            if (usuario == null)
                return Unauthorized("E-mail ou senha inválidos.");

            var token = GerarToken(usuario);

            // 🔥 DEBUG: Verificar se o token está sendo gerado
            Console.WriteLine($"=== TOKEN GERADO ===");
            Console.WriteLine($"Token: {token}");
            Console.WriteLine($"Usuário: {usuario.Name}");
            Console.WriteLine($"Role: {usuario.Role}");

            return Ok(new { token, usuario.Name, usuario.Role });
        }

        private string GerarToken(Usuario usuario)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 🔥 CORREÇÃO: Garantir que não sejam nulos
            var claims = new[]
            {
                new Claim("id", usuario.Id.ToString() ?? "0"),
                new Claim(ClaimTypes.Name, usuario.Name ?? ""),
                new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                new Claim(ClaimTypes.Role, usuario.Role ?? "usuario")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

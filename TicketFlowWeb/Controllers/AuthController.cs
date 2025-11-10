using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketFlowWeb.Models;

namespace TicketFlowWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verifica se email já existe
            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (usuarioExistente != null)
            {
                ModelState.AddModelError("Email", "Este email já está cadastrado");
                return View(model);
            }

            // Converte RegisterViewModel para UsuarioModel
            var usuario = new UsuarioModel
            {
                Nome = model.Nome,
                Email = model.Email,
                Senha = model.Senha
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Cadastro realizado com sucesso! Faça login para continuar.";
            return RedirectToAction("Login");
        }

        // GET: /Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Busca o usuário pelo email (sem criptografia por enquanto)
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Senha == model.Senha);

            if (usuario == null)
            {
                TempData["Erro"] = "Email ou senha inválidos!";
                return View(model);
            }

            // Login bem-sucedido - vamos implementar sessão
            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome ?? "usuario");
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email ?? "");
            HttpContext.Session.SetString("UsuarioRole", usuario.Role ?? "usuario");

            TempData["Mensagem"] = $"Bem-vindo, {usuario.Nome}!";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Mensagem"] = "Logout realizado com sucesso!";
            return RedirectToAction("Login");
        }
    }
}
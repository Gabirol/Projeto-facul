using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketFlowWeb.Models;

namespace TicketFlowWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                return RedirectToAction("Login", "Auth");
            }

            var usuarioId = int.Parse(usuarioIdStr);
            var usuarioNome = HttpContext.Session.GetString("UsuarioNome") ?? "Usuário";

            ViewBag.UsuarioNome = usuarioNome;
            ViewBag.TotalChamados = await _context.Chamados.CountAsync();
            ViewBag.MeusChamados = await _context.Chamados
                .Where(c => c.UsuarioId == usuarioId)
                .CountAsync();
            ViewBag.ChamadosAbertos = await _context.Chamados
                .Where(c => c.Status == "aberto")
                .CountAsync();
            ViewBag.ChamadosAndamento = await _context.Chamados
                .Where(c => c.Status == "andamento")
                .CountAsync();

            return View();
        }
    }
}
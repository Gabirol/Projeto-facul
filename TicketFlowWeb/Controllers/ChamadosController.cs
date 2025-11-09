using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketFlowWeb.Models;

namespace TicketFlowWeb.Controllers
{
    public class ChamadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChamadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /Chamados/Criar
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarChamadoViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Dados inválidos" });
                }

                // Pega o ID do usuário logado da sessão
                var usuarioId = HttpContext.Session.GetString("UsuarioId");
                if (string.IsNullOrEmpty(usuarioId))
                {
                    return Unauthorized(new { success = false, message = "Usuário não logado" });
                }

                var chamado = new ChamadoModel
                {
                    Titulo = model.Titulo,
                    Categoria = model.Categoria,
                    Descricao = model.Descricao,
                    Prioridade = model.Prioridade,
                    Status = "aberto",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UsuarioId = int.Parse(usuarioId)
                };

                _context.Chamados.Add(chamado);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Chamado criado com sucesso!", id = chamado.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro: {ex.Message}" });
            }
        }

        // GET: /Chamados/MeusChamados
        public async Task<IActionResult> MeusChamados()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var chamados = await _context.Chamados
                .Where(c => c.UsuarioId == int.Parse(usuarioId))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(chamados);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var chamado = await _context.Chamados
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
            {
                return NotFound();
            }

            return View(chamado);
        }
    }

    public class CriarChamadoViewModel
    {
        public string Titulo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Prioridade { get; set; } = "media";
    }
}
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

        // === MÉTODOS DE LISTAGEM ===

        // GET: /Chamados/ListarChamados - Todos os chamados
        public async Task<IActionResult> ListarChamados()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Login", "Auth");

            var chamados = await _context.Chamados
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(chamados);
        }

        // GET: /Chamados/MeusChamados - Apenas do usuário logado
        public async Task<IActionResult> MeusChamados()
        {
            if (!UsuarioLogado())
                return RedirectToAction("Login", "Auth");

            var chamados = await _context.Chamados
                .Where(c => c.UsuarioId == GetUsuarioId())
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(chamados);
        }

        // === MÉTODOS DE CRIAÇÃO ===

        // POST: /Chamados/Criar
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarChamadoViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dados inválidos" });

                if (!UsuarioLogado())
                    return Unauthorized(new { success = false, message = "Usuário não logado" });

                var chamado = new ChamadoModel
                {
                    Titulo = model.Titulo,
                    Categoria = model.Categoria,
                    Descricao = model.Descricao,
                    Prioridade = model.Prioridade,
                    Status = "aberto",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UsuarioId = GetUsuarioId()
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

        // === MÉTODOS DE DETALHES ===

        // GET: /Chamados/Detalhes/{id}
        public async Task<IActionResult> Detalhes(int id)
        {
            var chamado = await _context.Chamados
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
                return NotFound();

            return View(chamado);
        }

        private bool UsuarioLogado()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId"));
        }

        private int GetUsuarioId()
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
                return 0;

            return int.Parse(usuarioIdStr);
        }

        // === MÉTODOS DE EDIÇÃO ===

        // GET: /Chamados/Editar/{id}
        public async Task<IActionResult> Editar(int id)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Login", "Auth");

            var chamado = await _context.Chamados
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
                return NotFound();

            // Verifica se o usuário é o dono do chamado
            if (chamado.UsuarioId != GetUsuarioId())
            {
                TempData["Erro"] = "Você só pode editar seus próprios chamados.";
                return RedirectToAction("ListarChamados");
            }

            return View(chamado);
        }

        // POST: /Chamados/Editar/{id}
        [HttpPost]
        public async Task<IActionResult> Editar(int id, ChamadoModel model)
        {
            if (!UsuarioLogado())
                return RedirectToAction("Login", "Auth");

            var chamado = await _context.Chamados
                .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == GetUsuarioId());

            if (chamado == null)
            {
                TempData["Erro"] = "Chamado não encontrado ou você não tem permissão para editá-lo.";
                return RedirectToAction("ListarChamados");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Atualiza apenas os campos permitidos
            chamado.Titulo = model.Titulo;
            chamado.Descricao = model.Descricao;
            chamado.Categoria = model.Categoria;
            chamado.Prioridade = model.Prioridade;
            chamado.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Chamado atualizado com sucesso!";
            return RedirectToAction("ListarChamados");
        }

        // === MÉTODOS DE EXCLUSÃO ===

        // POST: /Chamados/Excluir/{id}
        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                if (!UsuarioLogado())
                    return Unauthorized(new { success = false, message = "Usuário não logado" });

                var chamado = await _context.Chamados
                    .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == GetUsuarioId());

                if (chamado == null)
                    return NotFound(new { success = false, message = "Chamado não encontrado" });

                _context.Chamados.Remove(chamado);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Chamado excluído com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro: {ex.Message}" });
            }
        }
    }

    // ViewModel para criação de chamados
    public class CriarChamadoViewModel
    {
        public string Titulo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Prioridade { get; set; } = "media";
    }
}
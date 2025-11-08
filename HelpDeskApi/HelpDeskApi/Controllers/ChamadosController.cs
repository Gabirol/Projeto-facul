using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDeskApi.Data;
using HelpDeskApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace HelpDeskApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChamadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadosController(AppDbContext context)
        {
            _context = context;
        }

        // Autenticação rolers
        [Authorize]
        [HttpGet("teste")]
        public IActionResult ListarChamados()
        {
            return Ok("Chamados disponíveis!");
        }

        [Authorize(Roles = "Tecnico")]
        [HttpPut("atualizar-status/{id}")]
        public IActionResult AtualizarStatus(int id)
        {
            return Ok($"Chamado {id} atualizado pelo técnico!");
        }

        // GET: api/Chamados - Mostrar chamados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chamado>>> GetChamados()
        {
            // Recupera o ID do usuário autenticado (armazenado no token JWT)
            var userIdClaim = User.FindFirst("id")?.Value; // ou "id" dependendo do token que você gera
            if (userIdClaim == null)
                return Unauthorized("Usuário não identificado no token.");

            int userId = int.Parse(userIdClaim);

            // Cria a query base incluindo as relações
            IQueryable<Chamado> query = _context.Chamados
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Bot);

            // Se for técnico → vê todos os chamados
            if (User.IsInRole("Tecnico"))
            {
                return await query.ToListAsync();
            }

            // Caso contrário → vê apenas os chamados que ele criou
            var meusChamados = await query
                .Where(c => c.UsuarioId == userId)
                .ToListAsync();

            return meusChamados;
        }

        // GET: api/Chamados/{id} - Mostrar chamado por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Chamado>> GetChamado(int id)
        {
            var chamado = await _context.Chamados
                .Include(c => c.Usuario)
                .Include(c => c.Tecnico)
                .Include(c => c.Bot)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
                return NotFound();

            return chamado;
        }

        // PUT personalizado – Fechar chamado
        [Authorize(Roles = "Tecnico")]
        [HttpPut("{id}/fechar")]
        public async Task<IActionResult> FecharChamado(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            if (chamado.Status == "Resolvido")
                return BadRequest("Este chamado já está fechado.");

            chamado.Status = "Resolvido";
            chamado.DataFechamento = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Chamado fechado com sucesso.",
                chamado.Id,
                chamado.Titulo,
                chamado.Status,
                chamado.DataFechamento
            });
        }

        // POST: api/Chamados - Criar chamado
        [HttpPost]
        public async Task<ActionResult<Chamado>> PostChamado(Chamado chamado)
        {
            // Configura valores padrão
            chamado.DataAbertura = DateTime.Now;
            if (string.IsNullOrEmpty(chamado.Status))
                chamado.Status = "Em andamento";

            // Verifica se o usuário existe
            var usuario = await _context.Usuarios.FindAsync(chamado.UsuarioId);
            if (usuario == null)
                return BadRequest("Usuário informado não existe.");

            // Verifica se o técnico (caso enviado) existe
            if (chamado.TecnicoId.HasValue)
            {
                var tecnico = await _context.Tecnicos.FindAsync(chamado.TecnicoId.Value);
                if (tecnico == null)
                    return BadRequest("Técnico informado não existe.");
            }

            _context.Chamados.Add(chamado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChamado), new { id = chamado.Id }, chamado);
        }

        // DELETE: api/Chamados/{id}
        [Authorize(Roles = "Tecnico")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChamado(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);

            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            _context.Chamados.Remove(chamado);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Chamado excluído com sucesso.",
                chamado.Id,
                chamado.Titulo,
                chamado.Status
            });
        }
        private bool ChamadoExists(int id)
        {
            return _context.Chamados.Any(e => e.Id == id);
        }

    }
}

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chamado>>> GetChamados()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim == null)
                return Unauthorized("Usuário não identificado no token.");

            int userId = int.Parse(userIdClaim);

            // 🔥 CORREÇÃO: Busca simples sem includes
            IQueryable<Chamado> query = _context.Chamados;

            // Se for técnico → vê todos os chamados
            if (User.IsInRole("Tecnico"))
            {
                return await query.ToListAsync();
            }

            // Caso contrário → vê apenas os chamados que ele criou
            var meusChamados = await query
                .Where(c => c.usuario_id == userId)
                .ToListAsync();

            return meusChamados;
        }

        // GET: api/Chamados/{id} - Mostrar chamado por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Chamado>> GetChamado(int id)
        {
            var chamado = await _context.Chamados
                .FirstOrDefaultAsync(c => c.id == id);

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

            // 🔥 CORREÇÃO: Usar propriedades minúsculas
            if (chamado.status == "Resolvido")
                return BadRequest("Este chamado já está fechado.");

            chamado.status = "Resolvido";
            chamado.updated_at = DateTime.Now; // 🔥 Usar updated_at em vez de DataFechamento

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Chamado fechado com sucesso.",
                chamado.id,           // 🔥 minúsculo
                chamado.titulo,       // 🔥 minúsculo  
                chamado.status,       // 🔥 minúsculo
                dataFechamento = chamado.updated_at
            });
        }

        [HttpPost]
        public async Task<ActionResult<Chamado>> PostChamado(Chamado chamado)
        {
            try
            {
                Console.WriteLine("=== INÍCIO POST CHAMADO ===");

                if (chamado == null)
                    return BadRequest("Dados do chamado são nulos.");

                // 🔥 CORREÇÃO: Validações com propriedades minúsculas
                if (string.IsNullOrWhiteSpace(chamado.titulo))
                    return BadRequest("Título é obrigatório.");

                if (string.IsNullOrWhiteSpace(chamado.descricao))
                    return BadRequest("Descrição é obrigatória.");

                if (chamado.usuario_id == null || chamado.usuario_id <= 0)
                    return BadRequest("ID do usuário é inválido.");

                // Configurar valores padrão
                chamado.created_at = DateTime.Now;
                chamado.updated_at = DateTime.Now;

                if (string.IsNullOrEmpty(chamado.status))
                    chamado.status = "Aberto";

                if (string.IsNullOrEmpty(chamado.prioridade))
                    chamado.prioridade = "Normal";

                // Verificar se o usuário existe
                var usuario = await _context.Usuarios.FindAsync(chamado.usuario_id);
                if (usuario == null)
                    return BadRequest("Usuário informado não existe.");

                Console.WriteLine($"Salvando chamado: {chamado.titulo}");

                // Salvar no banco
                _context.Chamados.Add(chamado);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Chamado salvo com ID: {chamado.id}");

                return CreatedAtAction(nameof(GetChamado), new { id = chamado.id }, chamado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex.Message}");
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
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
                chamado.id,       // 🔥 minúsculo
                chamado.titulo,   // 🔥 minúsculo
                chamado.status    // 🔥 minúsculo
            });
        }

        private bool ChamadoExists(int id)
        {
            return _context.Chamados.Any(e => e.id == id); // 🔥 minúsculo
        }
    }
}
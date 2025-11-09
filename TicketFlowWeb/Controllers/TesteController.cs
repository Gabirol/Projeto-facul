using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketFlowWeb.Models;

namespace TicketFlowWeb.Controllers
{
    public class TesteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TesteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult TesteConexao()
        {
            try
            {
                // Testa se consegue conectar ao banco
                bool podeConectar = _context.Database.CanConnect();

                if (podeConectar)
                {
                    ViewBag.Mensagem = " Conexao com SQL Server bem-sucedida!";
                    ViewBag.Cor = "green";
                }
                else
                {
                    ViewBag.Mensagem = "? Nao foi possível conectar ao banco";
                    ViewBag.Cor = "red";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensagem = $" Erro na conexao: {ex.Message}";
                ViewBag.Cor = "red";
            }

            return View();
        }
    }
}
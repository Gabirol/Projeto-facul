using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TicketFlowWeb.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<ChamadoModel> Chamados { get; set; }
    }
}
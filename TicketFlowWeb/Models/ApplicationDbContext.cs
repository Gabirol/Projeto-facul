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

        // Mapeie suas tabelas existentes do banco HelpDeskDB
        public DbSet<UsuarioModel> Usuarios { get; set; }

        // Adicione outras DbSets conforme suas tabelas:
        // public DbSet<Chamado> Chamados { get; set; }
        // public DbSet<Departamento> Departamentos { get; set; }
    }
}
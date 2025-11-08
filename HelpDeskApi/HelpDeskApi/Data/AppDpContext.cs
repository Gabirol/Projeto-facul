using Microsoft.EntityFrameworkCore;
using HelpDeskApi.Models;

namespace HelpDeskApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Bot> Bots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Usuário - Chamados
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Chamados)
                .HasForeignKey(c => c.UsuarioId)
                .IsRequired();

            // Técnico - Chamados
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Tecnico)
                .WithMany(t => t.Chamados)
                .HasForeignKey(c => c.TecnicoId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false); ;

            // Bot - Chamados
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Bot)
                .WithMany(b => b.Chamados)
                .HasForeignKey(c => c.BotId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false); ;
        }
    }
}

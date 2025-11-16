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
            modelBuilder.Entity<Chamado>(entity =>
            {
                entity.ToTable("chamados");
                entity.HasKey(e => e.id);

                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.usuario_id).HasColumnName("usuario_id");
                entity.Property(e => e.titulo).HasColumnName("titulo");
                entity.Property(e => e.descricao).HasColumnName("descricao");
                entity.Property(e => e.categoria).HasColumnName("categoria");
                entity.Property(e => e.prioridade).HasColumnName("prioridade");
                entity.Property(e => e.status).HasColumnName("status");
                entity.Property(e => e.created_at).HasColumnName("created_at");
                entity.Property(e => e.updated_at).HasColumnName("updated_at");
            });

        }
    }
}

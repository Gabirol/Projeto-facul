using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketFlowWeb.Models
{
    [Table("chamados")]
    public class ChamadoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [Column("titulo")]
        [StringLength(255)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [Column("descricao")]
        [StringLength(255)]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria é obrigatória")]
        [Column("categoria")]
        [StringLength(255)]
        public string Categoria { get; set; } = string.Empty;

        [Column("prioridade")]
        [StringLength(255)]
        public string Prioridade { get; set; } = "media";

        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = "aberto";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Campo adicional para associar ao usuário (se sua tabela não tem, podemos adicionar depois)
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
    }
}
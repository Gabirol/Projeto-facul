using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketFlowWeb.Models
{
    [Table("Usuarios")]
    public class UsuarioModel
    {
        [Key]
        [Column("id")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Column("Name")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Column("Email")] 
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        [Column("senha")]
        public string Senha { get; set; }

        [Column("Role")]
        public string Role { get; set; } = "usuario";
    }
}
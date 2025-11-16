namespace HelpDeskApi.Models
{
    public class Usuario
    {
        public int Id { get; set;  }
        public string? Name { get; set; } = string.Empty;
        public string? Email {  get; set; } = string.Empty;
        public string? senha {  get; set; } = string.Empty;
        public string? Role { get; set; } = "Usuario";

        public ICollection<Chamado>? Chamados { get; set; }
    }
}

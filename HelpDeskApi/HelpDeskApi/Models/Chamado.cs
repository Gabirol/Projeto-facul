namespace HelpDeskApi.Models
{
    public class Chamado
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status {  get; set; } = "Em andamento";
        public string Prioridade { get; set; } = "Normal";

        public DateTime DataAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int? TecnicoId { get; set; }
        public Tecnico? Tecnico { get; set; }

        public int? BotId {  get; set; }
        public Bot? Bot { get; set; }
    }
}

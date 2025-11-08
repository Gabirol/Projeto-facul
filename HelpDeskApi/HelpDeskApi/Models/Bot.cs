namespace HelpDeskApi.Models
{
    public class Bot
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public ICollection<Chamado>? Chamados { get; set; }
    }
}

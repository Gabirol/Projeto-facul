namespace HelpDeskApi.Models
{
    public class Chamado
    {
        public int id { get; set; }
        public int? usuario_id { get; set; }
        public string? titulo { get; set; }
        public string? descricao { get; set; }
        public string? categoria { get; set; }
        public string? prioridade { get; set; }
        public string? status { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}

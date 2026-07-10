namespace FacilAssist.Web.Models
{
    public class ClienteRequest
    {
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public int SituacaoClienteId { get; set; }
    }
}

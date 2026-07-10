namespace FacilAssist.API.DTOs
{
    public class ClienteInputDto
    {
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public int SituacaoClienteId { get; set; }
    }
}

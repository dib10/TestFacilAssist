namespace FacilAssist.API.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public int SituacaoClienteId { get; set; }

        public string SituacaoDescricao { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace FacilAssist.Web.Models
{
    public class ClienteFormModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Informe o CPF.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "Informe a data de nascimento.")]
        [Display(Name = "Data de nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "Informe o sexo.")]
        [RegularExpression("^[MF]$", ErrorMessage = "Informe M ou F.")]
        public string? Sexo { get; set; }

        [Display(Name = "Situação")]
        public int SituacaoClienteId { get; set; } = 1;
    }
}

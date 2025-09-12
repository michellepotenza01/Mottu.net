using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    public class PatioDto
    {
        [Required(ErrorMessage = "O nome do p�tio � obrigat�rio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do p�tio deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "O nome do p�tio deve conter apenas letras, n�meros e espa�os.")]
        [SwaggerSchema(Description = "Nome �nico do p�tio", Example = "P�tio Centro")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "A localiza��o do p�tio � obrigat�ria.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "A localiza��o deve ter entre 10 e 200 caracteres.")]
        [SwaggerSchema(Description = "Localiza��o completa do p�tio", Example = "Rua das Flores, 123 - Centro - S�o Paulo/SP")]
        public string Localizacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O n�mero total de vagas � obrigat�rio.")]
        [Range(1, 1000, ErrorMessage = "O p�tio deve ter entre 1 e 1000 vagas.")]
        [SwaggerSchema(Description = "N�mero total de vagas dispon�veis no p�tio", Example = 50)]
        public int VagasTotais { get; set; }

        [Required(ErrorMessage = "O n�mero de vagas ocupadas � obrigat�rio.")]
        [Range(0, 1000, ErrorMessage = "As vagas ocupadas devem estar entre 0 e 1000.")]
        [SwaggerSchema(Description = "N�mero de vagas atualmente ocupadas", Example = 25)]
        public int VagasOcupadas { get; set; }
    }
}
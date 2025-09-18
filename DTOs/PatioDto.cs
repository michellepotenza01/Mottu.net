using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    public class PatioDto
    {
        [Required(ErrorMessage = "O nome do p�tio � obrigat�rio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do p�tio deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "O nome do p�tio deve conter apenas letras, n�meros e espa�os.")]
        [SwaggerSchema("Nome �nico do p�tio")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "A localiza��o do p�tio � obrigat�ria.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "A localiza��o deve ter entre 10 e 200 caracteres.")]
        [SwaggerSchema("Localiza��o completa do p�tio")]
        public string Localizacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O n�mero total de vagas � obrigat�rio.")]
        [Range(1, 1000, ErrorMessage = "O p�tio deve ter entre 1 e 1000 vagas.")]
        [SwaggerSchema("N�mero total de vagas dispon�veis no p�tio")]
        public int VagasTotais { get; set; }
    }
}
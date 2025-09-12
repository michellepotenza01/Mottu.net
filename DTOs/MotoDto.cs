using System.ComponentModel.DataAnnotations;
using MottuApi.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    public class MotoDto
    {
        [Required(ErrorMessage = "A placa � obrigat�ria.")]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "A placa deve ter 7 caracteres no formato XXX-0000.")]
        [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Formato de placa inv�lido. Use: XXX-0000")]
        [SwaggerSchema(Description = "Placa da moto no formato XXX-0000", Example = "ABC-1234")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo da moto � obrigat�rio.")]
        [SwaggerSchema(Description = "Modelo da moto", Example = "MottuSport")]
        public ModeloMoto Modelo { get; set; }

        [Required(ErrorMessage = "O status da moto � obrigat�rio.")]
        [SwaggerSchema(Description = "Status atual da moto", Example = "Dispon�vel")]
        public StatusMoto Status { get; set; }

        [Required(ErrorMessage = "O setor de conserva��o � obrigat�rio.")]
        [SwaggerSchema(Description = "Setor de conserva��o da moto", Example = "Bom")]
        public SetorMoto Setor { get; set; }

        [Required(ErrorMessage = "O p�tio onde a moto est� alocada � obrigat�rio.")]
        [StringLength(50, ErrorMessage = "O nome do p�tio deve ter no m�ximo 50 caracteres.")]
        [SwaggerSchema(Description = "P�tio onde a moto est� alocada", Example = "P�tio Centro")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "O funcion�rio respons�vel � obrigat�rio.")]
        [StringLength(50, ErrorMessage = "O usu�rio do funcion�rio deve ter no m�ximo 50 caracteres.")]
        [SwaggerSchema(Description = "Usu�rio do funcion�rio respons�vel pela moto", Example = "maria_alm")]
        public string UsuarioFuncionario { get; set; } = string.Empty;
    }
}
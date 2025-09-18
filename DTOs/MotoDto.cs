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
        [SwaggerSchema("Placa da moto no formato XXX-0000")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo da moto � obrigat�rio.")]
        [SwaggerSchema("Modelo da moto")]
        public ModeloMoto Modelo { get; set; }

        [Required(ErrorMessage = "O status da moto � obrigat�rio.")]
        [SwaggerSchema("Status atual da moto")]
        public StatusMoto Status { get; set; }

        [Required(ErrorMessage = "O setor de conserva��o � obrigat�rio.")]
        [SwaggerSchema("Setor de conserva��o da moto")]
        public SetorMoto Setor { get; set; }

        [Required(ErrorMessage = "O p�tio onde a moto est� alocada � obrigat�rio.")]
        [StringLength(50, ErrorMessage = "O nome do p�tio deve ter no m�ximo 50 caracteres.")]
        [SwaggerSchema("P�tio onde a moto est� alocada")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "O funcion�rio respons�vel � obrigat�rio.")]
        [StringLength(50, ErrorMessage = "O usu�rio do funcion�rio deve ter no m�ximo 50 caracteres.")]
        [SwaggerSchema("Usu�rio do funcion�rio respons�vel pela moto")]
        public string UsuarioFuncionario { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    public class ClienteDto
    {
        [Required(ErrorMessage = "O nome de usu�rio do cliente � obrigat�rio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O usu�rio deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "O usu�rio deve conter apenas letras, n�meros e underscore.")]
        [SwaggerSchema("Nome de usu�rio �nico do cliente")]
        public string UsuarioCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome completo do cliente � obrigat�rio.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres.")]
        [SwaggerSchema("Nome completo do cliente")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do cliente � obrigat�ria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no m�nimo 6 caracteres.")]
        [SwaggerSchema("Senha do cliente (m�nimo 6 caracteres)")]
        public string Senha { get; set; } = string.Empty;

        [SwaggerSchema("Placa da moto associada ao cliente (opcional)")]
        [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Formato de placa inv�lido. Use: XXX-0000")]
        public string? MotoPlaca { get; set; }
    }
}
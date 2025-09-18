using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    public class FuncionarioDto
    {
        [Required(ErrorMessage = "O nome de usu�rio do funcion�rio � obrigat�rio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O usu�rio deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "O usu�rio deve conter apenas letras e n�meros.")]
        [SwaggerSchema("Nome de usu�rio �nico do funcion�rio")]
        public string UsuarioFuncionario { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome completo do funcion�rio � obrigat�rio.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres.")]
        [SwaggerSchema("Nome completo do funcion�rio")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do funcion�rio � obrigat�ria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no m�nimo 6 caracteres.")]
        [SwaggerSchema("Senha do funcion�rio")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O p�tio de trabalho � obrigat�rio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do p�tio deve ter entre 3 e 50 caracteres.")]
        [SwaggerSchema("Nome do p�tio onde o funcion�rio trabalha")]
        public string NomePatio { get; set; } = string.Empty;
    }
}
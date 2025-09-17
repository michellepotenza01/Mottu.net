using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    public class FuncionarioDto
    {
        [Required(ErrorMessage = "O nome de usuário do funcionário é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O usuário deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "O usuário deve conter apenas letras e números.")]
        [SwaggerSchema(Description = "Nome de usuário único do funcionário", Example = "MichellePtz")]
        public string UsuarioFuncionario { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome completo do funcionário é obrigatório.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres.")]
        [SwaggerSchema(Description = "Nome completo do funcionário", Example = "Michelle Potenza")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do funcionário é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [SwaggerSchema(Description = "Senha do funcionário", Example = "func123")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O pátio de trabalho é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do pátio deve ter entre 3 e 50 caracteres.")]
        [SwaggerSchema(Description = "Nome do pátio onde o funcionário trabalha", Example = "Pátio Osasco")]
        public string NomePatio { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MottuApi.Models
{
    [Table("Funcionarios")]
    public class Funcionario
    {
        [Key]
        [Required(ErrorMessage = "O nome de usuario do funcionario obrigatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O usuario deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "O usuario deve conter apenas letras, numeros e underscore.")]
        [SwaggerSchema("Nome de usuario unico do funcionario")]
        public string UsuarioFuncionario { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome completo do funcionario obrigatorio.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres.")]
        [SwaggerSchema("Nome completo do funcionario")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do funcionario obrigatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no minimo 6 caracteres.")]
        [SwaggerSchema("Senha do funcionario")]
        [JsonIgnore]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O patio de trabalho obrigatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do patio deve ter entre 3 e 50 caracteres.")]
        [SwaggerSchema("Nome do patio onde o funcionario trabalha")]
        public string NomePatio { get; set; } = string.Empty;

        [ForeignKey("NomePatio")]
        [JsonIgnore]
        [SwaggerSchema("Patio onde o funcionario esta alocado")]
        public virtual Patio Patio { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Moto>? Motos { get; set; }
    }
}
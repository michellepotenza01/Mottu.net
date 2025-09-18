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
        [Required(ErrorMessage = "O nome de usuário do funcionário é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O usuário deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "O usuário deve conter apenas letras, números e underscore.")]
        [SwaggerSchema("Nome de usuário único do funcionário")]
        public string UsuarioFuncionario { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome completo do funcionário é obrigatório.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres.")]
        [SwaggerSchema("Nome completo do funcionário")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do funcionário é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [SwaggerSchema("Senha do funcionário")]
        [JsonIgnore]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O pátio de trabalho é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do pátio deve ter entre 3 e 50 caracteres.")]
        [SwaggerSchema("Nome do pátio onde o funcionário trabalha")]
        public string NomePatio { get; set; } = string.Empty;

        [ForeignKey("NomePatio")]
        [JsonIgnore]
        [SwaggerSchema("Pátio onde o funcionário está alocado")]
        public virtual Patio Patio { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Moto>? Motos { get; set; }
    }
}
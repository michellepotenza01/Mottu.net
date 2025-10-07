using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MottuApi.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        [Required(ErrorMessage = "O nome de usuario do cliente obrigatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O usuario deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "O usuario deve conter apenas letras, numeros e underscore.")]
        [SwaggerSchema("Nome de usuario unico do cliente")]
        public string UsuarioCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome completo do cliente e obrigatorio.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres.")]
        [SwaggerSchema("Nome completo do cliente")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha do cliente e obrigatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no minimo 6 caracteres.")]
        [SwaggerSchema("Senha do cliente (minimo 6 caracteres)")]
        [JsonIgnore]
        public string Senha { get; set; } = string.Empty;

        [SwaggerSchema("Placa da moto associada ao cliente (opcional)")]
        [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Formato de placa invalido. Use: XXX-0000")]
        public string? MotoPlaca { get; set; }

        [ForeignKey("MotoPlaca")]
        [SwaggerSchema("Moto associada ao cliente")]
        public virtual Moto? Moto { get; set; }
    }
}
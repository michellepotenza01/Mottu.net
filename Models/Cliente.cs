using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa um cliente no sistema.
    /// </summary>
    public class Cliente
    {
        [Key]
        [Required(ErrorMessage = "O usuário do cliente é obrigatório.")]
        [StringLength(450)]
        [SwaggerSchema(Description = "Nome de usuário único para o cliente.")]
        public string UsuarioCliente { get; set; }

        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome completo do cliente.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A senha do cliente é obrigatória.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Senha do cliente.")]
        public string Senha { get; set; }

        [SwaggerSchema(Description = "Placa da moto associada ao cliente.")]
        public string? MotoPlaca { get; set; }

        public Cliente()
        {
            UsuarioCliente = string.Empty;
            Nome = string.Empty;
            Senha = string.Empty;
        }
    }
}

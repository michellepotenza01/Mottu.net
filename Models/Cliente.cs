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
        [Required(ErrorMessage = "O usu�rio do cliente � obrigat�rio.")]
        [StringLength(450)]
        [SwaggerSchema(Description = "Nome de usu�rio �nico para o cliente.")]
        public string UsuarioCliente { get; set; }

        [Required(ErrorMessage = "O nome do cliente � obrigat�rio.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome completo do cliente.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A senha do cliente � obrigat�ria.")]
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

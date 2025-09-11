using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    /// <summary>
    /// DTO para representar um cliente.
    /// </summary>
    public class ClienteDto
    {
        /// <summary>
        /// Nome de usu�rio �nico para o cliente.
        /// </summary>
        [Required(ErrorMessage = "O usu�rio do cliente � obrigat�rio.")]
        [StringLength(450)]
        [SwaggerSchema(Description = "Nome de usu�rio �nico para o cliente.")]
        public string UsuarioCliente { get; set; }

        /// <summary>
        /// Nome completo do cliente.
        /// </summary>
        [Required(ErrorMessage = "O nome do cliente � obrigat�rio.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome completo do cliente.")]
        public string Nome { get; set; }

        /// <summary>
        /// Senha do cliente.
        /// </summary>
        [Required(ErrorMessage = "A senha do cliente � obrigat�ria.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Senha do cliente.")]
        public string Senha { get; set; }

        /// <summary>
        /// Placa da moto associada ao cliente, se houver.
        /// </summary>
        [SwaggerSchema(Description = "Placa da moto associada ao cliente.")]
        public string? MotoPlaca { get; set; }

        public ClienteDto()
        {
            UsuarioCliente = string.Empty;
            Nome = string.Empty;
            Senha = string.Empty;
        }
    }
}


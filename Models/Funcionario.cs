using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa um funcionário que trabalha em um pátio.
    /// </summary>
    public class Funcionario
    {
        /// <summary>
        /// Nome de usuário único para o funcionário.
        /// </summary>
        [Key]
        [Required(ErrorMessage = "O usuário do funcionário é obrigatório.")]
        [StringLength(450)]
        [SwaggerSchema(Description = "Nome de usuário único para o funcionário.")]
        public string UsuarioFuncionario { get; set; }

        /// <summary>
        /// Nome completo do funcionário.
        /// </summary>
        [Required(ErrorMessage = "O nome do funcionário é obrigatório.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome completo do funcionário.")]
        public string Nome { get; set; }

        /// <summary>
        /// Senha do funcionário.
        /// </summary>
        [Required(ErrorMessage = "A senha do funcionário é obrigatória.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Senha do funcionário.")]
        public string Senha { get; set; }

        /// <summary>
        /// Nome do pátio onde o funcionário trabalha.
        /// </summary>
        [Required(ErrorMessage = "O nome do pátio é obrigatório.")]
        [SwaggerSchema(Description = "Nome do pátio onde o funcionário trabalha.")]
        public string NomePatio { get; set; }

        /// <summary>
        /// Relacionamento com o Pátio onde o funcionário está alocado.
        /// </summary>
        public Patio Patio { get; set; }

        public Funcionario()
        {
            UsuarioFuncionario = string.Empty;
            Nome = string.Empty;
            Senha = string.Empty;
            NomePatio = string.Empty;
            Patio = new Patio();
        }
    }
}

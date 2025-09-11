using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa um funcion�rio que trabalha em um p�tio.
    /// </summary>
    public class Funcionario
    {
        /// <summary>
        /// Nome de usu�rio �nico para o funcion�rio.
        /// </summary>
        [Key]
        [Required(ErrorMessage = "O usu�rio do funcion�rio � obrigat�rio.")]
        [StringLength(450)]
        [SwaggerSchema(Description = "Nome de usu�rio �nico para o funcion�rio.")]
        public string UsuarioFuncionario { get; set; }

        /// <summary>
        /// Nome completo do funcion�rio.
        /// </summary>
        [Required(ErrorMessage = "O nome do funcion�rio � obrigat�rio.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome completo do funcion�rio.")]
        public string Nome { get; set; }

        /// <summary>
        /// Senha do funcion�rio.
        /// </summary>
        [Required(ErrorMessage = "A senha do funcion�rio � obrigat�ria.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Senha do funcion�rio.")]
        public string Senha { get; set; }

        /// <summary>
        /// Nome do p�tio onde o funcion�rio trabalha.
        /// </summary>
        [Required(ErrorMessage = "O nome do p�tio � obrigat�rio.")]
        [SwaggerSchema(Description = "Nome do p�tio onde o funcion�rio trabalha.")]
        public string NomePatio { get; set; }

        /// <summary>
        /// Relacionamento com o P�tio onde o funcion�rio est� alocado.
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

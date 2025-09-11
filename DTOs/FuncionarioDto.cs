using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    /// <summary>
    /// DTO para representar um funcion�rio.
    /// </summary>
    public class FuncionarioDto
    {
        /// <summary>
        /// Nome de usu�rio �nico para o funcion�rio.
        /// </summary>
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
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome do p�tio onde o funcion�rio trabalha.")]
        public string NomePatio { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    /// <summary>
    /// DTO para representar um p�tio.
    /// </summary>
    public class PatioDto
    {
        /// <summary>
        /// Nome �nico do p�tio.
        /// </summary>
        [Required(ErrorMessage = "O nome do p�tio � obrigat�rio.")]
        [StringLength(450)]
        [SwaggerSchema(Description = "Nome do p�tio.")]
        public string NomePatio { get; set; }

        /// <summary>
        /// Localiza��o do p�tio.
        /// </summary>
        [Required(ErrorMessage = "A localiza��o do p�tio � obrigat�ria.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Localiza��o do p�tio.")]
        public string Localizacao { get; set; }

        /// <summary>
        ///  total de vagas no p�tio.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "O n�mero de vagas totais deve ser maior que zero.")]
        [Required(ErrorMessage = "O n�mero total de vagas � obrigat�rio.")]
        [SwaggerSchema(Description = "N�mero total de vagas no p�tio.")]
        public int VagasTotais { get; set; }

        /// <summary>
        /// vagas ocupadas no p�tio.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "O n�mero de vagas ocupadas deve ser maior ou igual a zero.")]
        [Required(ErrorMessage = "O n�mero de vagas ocupadas � obrigat�rio.")]
        [SwaggerSchema(Description = "N�mero de vagas ocupadas no p�tio.")]
        public int VagasOcupadas { get; set; }
    }
}

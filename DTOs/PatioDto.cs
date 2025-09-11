using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    /// <summary>
    /// DTO para representar um pátio.
    /// </summary>
    public class PatioDto
    {
        /// <summary>
        /// Nome único do pátio.
        /// </summary>
        [Required(ErrorMessage = "O nome do pátio é obrigatório.")]
        [StringLength(450)]
        [SwaggerSchema(Description = "Nome do pátio.")]
        public string NomePatio { get; set; }

        /// <summary>
        /// Localização do pátio.
        /// </summary>
        [Required(ErrorMessage = "A localização do pátio é obrigatória.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Localização do pátio.")]
        public string Localizacao { get; set; }

        /// <summary>
        ///  total de vagas no pátio.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "O número de vagas totais deve ser maior que zero.")]
        [Required(ErrorMessage = "O número total de vagas é obrigatório.")]
        [SwaggerSchema(Description = "Número total de vagas no pátio.")]
        public int VagasTotais { get; set; }

        /// <summary>
        /// vagas ocupadas no pátio.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "O número de vagas ocupadas deve ser maior ou igual a zero.")]
        [Required(ErrorMessage = "O número de vagas ocupadas é obrigatório.")]
        [SwaggerSchema(Description = "Número de vagas ocupadas no pátio.")]
        public int VagasOcupadas { get; set; }
    }
}

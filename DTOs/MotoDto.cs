using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    /// <summary>
    /// DTO para representar uma moto.
    /// </summary>
    public class MotoDto
    {
        /// <summary>
        /// Placa única da moto.
        /// </summary>
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(7, ErrorMessage = "A placa deve ter 7 caracteres.")]
        [SwaggerSchema(Description = "Placa da moto.")]
        public string Placa { get; set; }

        /// <summary>
        /// Modelo da moto.
        /// </summary>
        [Required(ErrorMessage = "O modelo da moto é obrigatório.")]
        [StringLength(100)]
        [SwaggerSchema(Description = "Modelo da moto.")]
        public string Modelo { get; set; }

        /// <summary>
        /// Status da moto (Disponível, Alugada, Manutenção).
        /// </summary>
        [Required(ErrorMessage = "O status da moto é obrigatório.")]
        [StringLength(100)]
        [RegularExpression("^(Disponível|Alugada|Manutenção)$", ErrorMessage = "Status inválido. Os valores válidos são: 'Disponível', 'Alugada', ou 'Manutenção'.")]
        [SwaggerSchema(Description = "Status da moto.")]
        public string Status { get; set; }

        /// <summary>
        /// Setor onde a moto está alocada.
        /// </summary>
        [Required(ErrorMessage = "O setor da moto é obrigatório.")]
        [StringLength(100)]
        [RegularExpression("^(Bom|Intermediário|Ruim)$", ErrorMessage = "Setor inválido. Os valores válidos são: 'Bom', 'Intermediário', ou 'Ruim'.")]
        [SwaggerSchema(Description = "Setor onde a moto está alocada.")]
        public string Setor { get; set; }

        /// <summary>
        /// Nome do pátio onde a moto está alocada.
        /// </summary>
        [Required(ErrorMessage = "O nome do pátio é obrigatório.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome do pátio onde a moto está alocada.")]
        public string NomePatio { get; set; }

        /// <summary>
        /// Nome de usuário do funcionário responsável pela moto.
        /// </summary>
        [Required(ErrorMessage = "O usuário do funcionário é obrigatório.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome de usuário do funcionário responsável pela moto.")]
        public string UsuarioFuncionario { get; set; }
    }
}

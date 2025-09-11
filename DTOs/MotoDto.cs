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
        /// Placa �nica da moto.
        /// </summary>
        [Required(ErrorMessage = "A placa � obrigat�ria.")]
        [StringLength(7, ErrorMessage = "A placa deve ter 7 caracteres.")]
        [SwaggerSchema(Description = "Placa da moto.")]
        public string Placa { get; set; }

        /// <summary>
        /// Modelo da moto.
        /// </summary>
        [Required(ErrorMessage = "O modelo da moto � obrigat�rio.")]
        [StringLength(100)]
        [SwaggerSchema(Description = "Modelo da moto.")]
        public string Modelo { get; set; }

        /// <summary>
        /// Status da moto (Dispon�vel, Alugada, Manuten��o).
        /// </summary>
        [Required(ErrorMessage = "O status da moto � obrigat�rio.")]
        [StringLength(100)]
        [RegularExpression("^(Dispon�vel|Alugada|Manuten��o)$", ErrorMessage = "Status inv�lido. Os valores v�lidos s�o: 'Dispon�vel', 'Alugada', ou 'Manuten��o'.")]
        [SwaggerSchema(Description = "Status da moto.")]
        public string Status { get; set; }

        /// <summary>
        /// Setor onde a moto est� alocada.
        /// </summary>
        [Required(ErrorMessage = "O setor da moto � obrigat�rio.")]
        [StringLength(100)]
        [RegularExpression("^(Bom|Intermedi�rio|Ruim)$", ErrorMessage = "Setor inv�lido. Os valores v�lidos s�o: 'Bom', 'Intermedi�rio', ou 'Ruim'.")]
        [SwaggerSchema(Description = "Setor onde a moto est� alocada.")]
        public string Setor { get; set; }

        /// <summary>
        /// Nome do p�tio onde a moto est� alocada.
        /// </summary>
        [Required(ErrorMessage = "O nome do p�tio � obrigat�rio.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome do p�tio onde a moto est� alocada.")]
        public string NomePatio { get; set; }

        /// <summary>
        /// Nome de usu�rio do funcion�rio respons�vel pela moto.
        /// </summary>
        [Required(ErrorMessage = "O usu�rio do funcion�rio � obrigat�rio.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome de usu�rio do funcion�rio respons�vel pela moto.")]
        public string UsuarioFuncionario { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa um pátio onde as motos são alocadas.
    /// </summary>
    public class Patio
    {
        [Key]
        [Required(ErrorMessage = "O nome do pátio é obrigatório.")]
        [StringLength(450)]
        [SwaggerSchema(Description = "Nome do pátio.")]
        public string NomePatio { get; set; }

        [Required(ErrorMessage = "A localização do pátio é obrigatória.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Localização do pátio.")]
        public string Localizacao { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O número de vagas totais deve ser maior que zero.")]
        [Required(ErrorMessage = "O número total de vagas é obrigatório.")]
        [SwaggerSchema(Description = "Número total de vagas no pátio.")]
        public int VagasTotais { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O número de vagas ocupadas deve ser maior ou igual a zero.")]
        [Required(ErrorMessage = "O número de vagas ocupadas é obrigatório.")]
        [SwaggerSchema(Description = "Número de vagas ocupadas no pátio.")]
        public int VagasOcupadas { get; set; }

        public List<Moto> Motos { get; set; }

        public Patio()
        {
            NomePatio = string.Empty;
            Localizacao = string.Empty;
        }
    }
}

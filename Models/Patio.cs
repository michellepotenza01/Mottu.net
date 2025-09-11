using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa um p�tio onde as motos s�o alocadas.
    /// </summary>
    public class Patio
    {
        [Key]
        [Required(ErrorMessage = "O nome do p�tio � obrigat�rio.")]
        [StringLength(450)]
        [SwaggerSchema(Description = "Nome do p�tio.")]
        public string NomePatio { get; set; }

        [Required(ErrorMessage = "A localiza��o do p�tio � obrigat�ria.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Localiza��o do p�tio.")]
        public string Localizacao { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O n�mero de vagas totais deve ser maior que zero.")]
        [Required(ErrorMessage = "O n�mero total de vagas � obrigat�rio.")]
        [SwaggerSchema(Description = "N�mero total de vagas no p�tio.")]
        public int VagasTotais { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O n�mero de vagas ocupadas deve ser maior ou igual a zero.")]
        [Required(ErrorMessage = "O n�mero de vagas ocupadas � obrigat�rio.")]
        [SwaggerSchema(Description = "N�mero de vagas ocupadas no p�tio.")]
        public int VagasOcupadas { get; set; }

        public List<Moto> Motos { get; set; }

        public Patio()
        {
            NomePatio = string.Empty;
            Localizacao = string.Empty;
        }
    }
}

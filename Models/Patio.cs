using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa um p�tio onde as motos s�o alocadas e gerenciadas.
    /// </summary>
    [Table("Patios")]
    public class Patio
    {
        [Key]
        [Required(ErrorMessage = "O nome do p�tio � obrigat�rio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do p�tio deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "O nome do p�tio deve conter apenas letras, n�meros e espa�os.")]
        [SwaggerSchema("Nome �nico do p�tio", Example = "P�tio Centro")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "A localiza��o do p�tio � obrigat�ria.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "A localiza��o deve ter entre 10 e 200 caracteres.")]
        [SwaggerSchema("Localiza��o completa do p�tio", Example = "Rua das Flores, 123 - Centro - S�o Paulo/SP")]
        public string Localizacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O n�mero total de vagas � obrigat�rio.")]
        [Range(1, 1000, ErrorMessage = "O p�tio deve ter entre 1 e 1000 vagas.")]
        [SwaggerSchema("N�mero total de vagas dispon�veis no p�tio", Example = 50)]
        public int VagasTotais { get; set; }

        [Required(ErrorMessage = "O n�mero de vagas ocupadas � obrigat�rio.")]
        [Range(0, 1000, ErrorMessage = "As vagas ocupadas devem estar entre 0 e 1000.")]
        [SwaggerSchema("N�mero de vagas atualmente ocupadas", Example = 25)]
        public int VagasOcupadas { get; set; }

        [SwaggerSchema("N�mero de vagas dispon�veis (calculado automaticamente)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int VagasDisponiveis => VagasTotais - VagasOcupadas;



        [SwaggerSchema("Lista de motos alocadas neste p�tio")]
        public List<Moto> Motos { get; set; } = new List<Moto>();


        public bool TemVagasDisponiveis() => VagasDisponiveis > 0;
        public bool PodeAlocarMoto() => TemVagasDisponiveis() && VagasDisponiveis >= 1;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models
{
    [Table("Patios")]
    public class Patio
    {
        [Key]
        [Required(ErrorMessage = "O nome do pátio é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do pátio deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "O nome do pátio deve conter apenas letras, números e espaços.")]
        [SwaggerSchema("Nome único do pátio", Example = "Pátio Centro")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "A localização do pátio é obrigatória.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "A localização deve ter entre 10 e 200 caracteres.")]
        [SwaggerSchema("Localização completa do pátio", Example = "Rua das Flores, 123 - Centro - São Paulo/SP")]
        public string Localizacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O número total de vagas é obrigatório.")]
        [Range(1, 1000, ErrorMessage = "O pátio deve ter entre 1 e 1000 vagas.")]
        [SwaggerSchema("Número total de vagas disponíveis no pátio", Example = 50)]
        public int VagasTotais { get; set; }

        [Required(ErrorMessage = "O número de vagas ocupadas é obrigatório.")]
        [Range(0, 1000, ErrorMessage = "As vagas ocupadas devem estar entre 0 e 1000.")]
        [SwaggerSchema("Número de vagas atualmente ocupadas", Example = 25)]
        public int VagasOcupadas { get; set; }

        [SwaggerSchema("Número de vagas disponíveis (calculado automaticamente)")]
        public int VagasDisponiveis => VagasTotais - VagasOcupadas;

        public virtual ICollection<Moto>? Motos { get; set; }
        public virtual ICollection<Funcionario>? Funcionarios { get; set; }
    }
}
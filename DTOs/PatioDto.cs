using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    public class PatioDto
    {
        [Required(ErrorMessage = "O nome do patio obrigatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do patio deve ter entre 3 e 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "O nome do patio deve conter apenas letras, numeros e espacos.")]
        [SwaggerSchema("Nome unico do patio")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "A localizacao do patio obrigatoria.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "A localizacao deve ter entre 10 e 200 caracteres.")]
        [SwaggerSchema("Localizacao completa do patio")]
        public string Localizacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O numero total de vagas obrigatorio.")]
        [Range(1, 1000, ErrorMessage = "O patio deve ter entre 1 e 1000 vagas.")]
        [SwaggerSchema("Numero total de vagas disponiveis no patio")]
        public int VagasTotais { get; set; }
    }
}
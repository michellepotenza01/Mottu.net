using System.ComponentModel.DataAnnotations;
using MottuApi3.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    public class MotoDto
    {
        [Required(ErrorMessage = "A placa obrigatoria.")]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "A placa deve ter 7 caracteres no formato XXX-0000.")]
        [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Formato de placa invalido. Use: XXX-0000")]
        [SwaggerSchema("Placa da moto no formato XXX-0000")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo da moto obrigatorio.")]
        [SwaggerSchema("Modelo da moto")]
        public ModeloMoto Modelo { get; set; }

        [Required(ErrorMessage = "O status da moto obrigatorio.")]
        [SwaggerSchema("Status atual da moto")]
        public StatusMoto Status { get; set; }

        [Required(ErrorMessage = "O setor de conservacao obrigatorio.")]
        [SwaggerSchema("Setor de conservacao da moto")]
        public SetorMoto Setor { get; set; }

        [Required(ErrorMessage = "O patio onde a moto esta alocada obrigatorio.")]
        [StringLength(50, ErrorMessage = "O nome do patio deve ter no maximo 50 caracteres.")]
        [SwaggerSchema("Patio onde a moto esta alocada")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "O funcionario responsavel obrigatorio.")]
        [StringLength(50, ErrorMessage = "O usuario do funcionario deve ter no maximo 50 caracteres.")]
        [SwaggerSchema("Usuario do funcionario responsavel pela moto")]
        public string UsuarioFuncionario { get; set; } = string.Empty;
    }
}
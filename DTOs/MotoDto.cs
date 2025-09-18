using System.ComponentModel.DataAnnotations;
using MottuApi.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.DTOs
{
    public class MotoDto
    {
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "A placa deve ter 7 caracteres no formato XXX-0000.")]
        [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Formato de placa inválido. Use: XXX-0000")]
        [SwaggerSchema("Placa da moto no formato XXX-0000")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo da moto é obrigatório.")]
        [SwaggerSchema("Modelo da moto")]
        public ModeloMoto Modelo { get; set; }

        [Required(ErrorMessage = "O status da moto é obrigatório.")]
        [SwaggerSchema("Status atual da moto")]
        public StatusMoto Status { get; set; }

        [Required(ErrorMessage = "O setor de conservação é obrigatório.")]
        [SwaggerSchema("Setor de conservação da moto")]
        public SetorMoto Setor { get; set; }

        [Required(ErrorMessage = "O pátio onde a moto está alocada é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome do pátio deve ter no máximo 50 caracteres.")]
        [SwaggerSchema("Pátio onde a moto está alocada")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "O funcionário responsável é obrigatório.")]
        [StringLength(50, ErrorMessage = "O usuário do funcionário deve ter no máximo 50 caracteres.")]
        [SwaggerSchema("Usuário do funcionário responsável pela moto")]
        public string UsuarioFuncionario { get; set; } = string.Empty;
    }
}
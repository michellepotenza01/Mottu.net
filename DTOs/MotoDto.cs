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
        [SwaggerSchema(Description = "Placa da moto no formato XXX-0000", Example = "ABC-1234")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo da moto é obrigatório.")]
        [SwaggerSchema(Description = "Modelo da moto", Example = "MottuSport")]
        public ModeloMoto Modelo { get; set; }

        [Required(ErrorMessage = "O status da moto é obrigatório.")]
        [SwaggerSchema(Description = "Status atual da moto", Example = "Disponível")]
        public StatusMoto Status { get; set; }

        [Required(ErrorMessage = "O setor de conservação é obrigatório.")]
        [SwaggerSchema(Description = "Setor de conservação da moto", Example = "Bom")]
        public SetorMoto Setor { get; set; }

        [Required(ErrorMessage = "O pátio onde a moto está alocada é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome do pátio deve ter no máximo 50 caracteres.")]
        [SwaggerSchema(Description = "Pátio onde a moto está alocada", Example = "Pátio Centro")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "O funcionário responsável é obrigatório.")]
        [StringLength(50, ErrorMessage = "O usuário do funcionário deve ter no máximo 50 caracteres.")]
        [SwaggerSchema(Description = "Usuário do funcionário responsável pela moto", Example = "maria_alm")]
        public string UsuarioFuncionario { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;
using MottuApi.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa uma moto cadastrada no sistema.
    /// </summary>
    public class Moto
    {
        [Key]
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(7)]
        [SwaggerSchema(Description = "Placa da moto.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O modelo da moto é obrigatório.")]
        [StringLength(100)]
        [SwaggerSchema(Description = "Modelo da moto.")]
        public ModeloMoto Modelo { get; set; }

        [Required(ErrorMessage = "O status da moto é obrigatório.")]
        [EnumDataType(typeof(StatusMoto))]
        [SwaggerSchema(Description = "Status da moto (Disponível, Alugada, Manutenção).")]
        public StatusMoto Status { get; set; }

        [Required(ErrorMessage = "O setor da moto é obrigatório.")]
        [EnumDataType(typeof(SetorMoto))]
        [SwaggerSchema(Description = "Setor onde a moto está alocada.")]
        public SetorMoto Setor { get; set; }

        [Required(ErrorMessage = "O nome do pátio é obrigatório.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome do pátio onde a moto está alocada.")]
        public string NomePatio { get; set; }

        [Required(ErrorMessage = "O usuário do funcionário é obrigatório.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Usuário do funcionário responsável pela moto.")]
        public string UsuarioFuncionario { get; set; }

        public Funcionario Funcionario { get; set; }
        public Patio Patio { get; set; }

        public Moto()
        {
            Placa = string.Empty;
            NomePatio = string.Empty;
            UsuarioFuncionario = string.Empty;
        }
    }
}

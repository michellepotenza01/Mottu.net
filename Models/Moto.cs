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
        [Required(ErrorMessage = "A placa � obrigat�ria.")]
        [StringLength(7)]
        [SwaggerSchema(Description = "Placa da moto.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O modelo da moto � obrigat�rio.")]
        [StringLength(100)]
        [SwaggerSchema(Description = "Modelo da moto.")]
        public ModeloMoto Modelo { get; set; }

        [Required(ErrorMessage = "O status da moto � obrigat�rio.")]
        [EnumDataType(typeof(StatusMoto))]
        [SwaggerSchema(Description = "Status da moto (Dispon�vel, Alugada, Manuten��o).")]
        public StatusMoto Status { get; set; }

        [Required(ErrorMessage = "O setor da moto � obrigat�rio.")]
        [EnumDataType(typeof(SetorMoto))]
        [SwaggerSchema(Description = "Setor onde a moto est� alocada.")]
        public SetorMoto Setor { get; set; }

        [Required(ErrorMessage = "O nome do p�tio � obrigat�rio.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Nome do p�tio onde a moto est� alocada.")]
        public string NomePatio { get; set; }

        [Required(ErrorMessage = "O usu�rio do funcion�rio � obrigat�rio.")]
        [StringLength(2000)]
        [SwaggerSchema(Description = "Usu�rio do funcion�rio respons�vel pela moto.")]
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

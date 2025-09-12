using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MottuApi.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models
{
    [Table("Motos")]
    public class Moto
    {
        [Key]
        [Required(ErrorMessage = "A placa da moto � obrigat�ria.")]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "A placa deve ter 7 caracteres no formato XXX-0000.")]
        [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Formato de placa inv�lido. Use: XXX-0000")]
        [SwaggerSchema("Placa da moto no formato XXX-0000", Example = "ABC-1234")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo da moto � obrigat�rio.")]
        [SwaggerSchema("Modelo da moto", Example = "MottuSport, MottuE ou MottuPop")]
        public ModeloMoto Modelo { get; set; }

        [Required(ErrorMessage = "O status da moto � obrigat�rio.")]
        [SwaggerSchema("Status atual da moto", Example = "Dispon�vel, Alugada ou Manuten��o")]
        public StatusMoto Status { get; set; } = StatusMoto.Dispon�vel;

        [Required(ErrorMessage = "O setor de conserva��o � obrigat�rio.")]
        [SwaggerSchema("Setor de conserva��o da moto", Example = "Bom, Intermedi�rio ou Ruim")]
        public SetorMoto Setor { get; set; } = SetorMoto.Bom;

        [Required(ErrorMessage = "O p�tio onde a moto est� alocada � obrigat�rio.")]
        [StringLength(50, ErrorMessage = "O nome do p�tio deve ter no m�ximo 50 caracteres.")]
        [SwaggerSchema("P�tio onde a moto est� alocada", Example = "P�tio Centro")]
        public string NomePatio { get; set; } = string.Empty;

        [Required(ErrorMessage = "O funcion�rio respons�vel � obrigat�rio.")]
        [StringLength(50, ErrorMessage = "O usu�rio do funcion�rio deve ter no m�ximo 50 caracteres.")]
        [SwaggerSchema("Usu�rio do funcion�rio respons�vel pela moto", Example = "maria_alm")]
        public string UsuarioFuncionario { get; set; } = string.Empty;

        [ForeignKey("NomePatio")]
        [SwaggerSchema("P�tio onde a moto est� alocada")]
        public virtual Patio Patio { get; set; } = null!;

        [ForeignKey("UsuarioFuncionario")]
        [SwaggerSchema("Funcion�rio respons�vel pela moto")]
        public virtual Funcionario Funcionario { get; set; } = null!;
    }
}
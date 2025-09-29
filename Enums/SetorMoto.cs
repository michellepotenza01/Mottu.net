using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi3.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [SwaggerSchema("Setores de conserva��o das motos")]
    public enum SetorMoto
    {
        [Display(Name = "Bom", Description = "Estado de conserva��o excelente")]
        Bom,

        [Display(Name = "Intermedi�rio", Description = "Estado de conserva��o regular")]
        Intermedi�rio,

        [Display(Name = "Ruim", Description = "Estado de conserva��o precisa de reparos")]
        Ruim
    }
}
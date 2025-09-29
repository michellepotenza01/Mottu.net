using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi3.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [SwaggerSchema("Setores de conservação das motos")]
    public enum SetorMoto
    {
        [Display(Name = "Bom", Description = "Estado de conservação excelente")]
        Bom,

        [Display(Name = "Intermediário", Description = "Estado de conservação regular")]
        Intermediário,

        [Display(Name = "Ruim", Description = "Estado de conservação precisa de reparos")]
        Ruim
    }
}
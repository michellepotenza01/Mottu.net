using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [SwaggerSchema("Modelos de moto disponíveis no sistema Mottu")]
    public enum ModeloMoto
    {
        [Display(Name = "MottuSport", Description = "Modelo esportivo de alta performance")]
        MottuSport,

        [Display(Name = "MottuE", Description = "Modelo elétrico eco-friendly")]
        MottuE,

        [Display(Name = "MottuPop", Description = "Modelo popular econômico")]
        MottuPop
    }
}
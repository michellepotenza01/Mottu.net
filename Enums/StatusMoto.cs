using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [SwaggerSchema("Status das motos no sistema")]
    public enum StatusMoto
    {
        [Display(Name = "Dispon�vel", Description = "Moto dispon�vel para aluguel")]
        Dispon�vel,

        [Display(Name = "Alugada", Description = "Moto atualmente alugada por um cliente")]
        Alugada,

        [Display(Name = "Manuten��o", Description = "Moto em manuten��o t�cnica")]
        Manuten��o
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [SwaggerSchema("Status das motos no sistema")]
    public enum StatusMoto
    {
        [Display(Name = "Disponível", Description = "Moto disponível para aluguel")]
        Disponível,

        [Display(Name = "Alugada", Description = "Moto atualmente alugada por um cliente")]
        Alugada,

        [Display(Name = "Manutenção", Description = "Moto em manutenção técnica")]
        Manutenção
    }
}
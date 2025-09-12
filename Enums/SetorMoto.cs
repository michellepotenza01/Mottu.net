using System.Text.Json.Serialization;

namespace MottuApi.Enums
{
    /// <summary>
    /// Enum para representar o setor onde a moto está alocada.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [SwaggerSchema(Description = "Setores de moto disponíveis no sistema")]
    public enum SetorMoto
    {
        /// <summary>
        /// Setor bom.
        /// </summary>
        Bom,

        /// <summary>
        /// Setor intermediário.
        /// </summary>
        Intermediário,

        /// <summary>
        /// Setor ruim.
        /// </summary>
        Ruim
    }
}

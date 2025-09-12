using System.Text.Json.Serialization;

namespace MottuApi.Enums
{
    /// <summary>
    /// Enum para representar o setor onde a moto est� alocada.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [SwaggerSchema(Description = "Setores de moto dispon�veis no sistema")]
    public enum SetorMoto
    {
        /// <summary>
        /// Setor bom.
        /// </summary>
        Bom,

        /// <summary>
        /// Setor intermedi�rio.
        /// </summary>
        Intermedi�rio,

        /// <summary>
        /// Setor ruim.
        /// </summary>
        Ruim
    }
}

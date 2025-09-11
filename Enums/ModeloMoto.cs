using System.Text.Json.Serialization;

namespace MottuApi.Enums
{
    /// <summary>
    /// Enum para representar o modelo da moto.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ModeloMoto
    {
        /// <summary>
        /// Modelo de moto MottuSport.
        /// </summary>
        MottuSport,

        /// <summary>
        /// Modelo de moto MottuE.
        /// </summary>
        MottuE,

        /// <summary>
        /// Modelo de moto MottuPop.
        /// </summary>
        MottuPop
    }
}

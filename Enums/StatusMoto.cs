using System.Text.Json.Serialization;

namespace MottuApi.Enums
{
    /// <summary>
    /// Enum para representar o status da moto.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusMoto
    {
        /// <summary>
        /// Moto dispon�vel para aluguel.
        /// </summary>
        Dispon�vel,

        /// <summary>
        /// Moto alugada.
        /// </summary>
        Alugada,

        /// <summary>
        /// Moto em manuten��o.
        /// </summary>
        Manuten��o
    }
}
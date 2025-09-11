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
        /// Moto disponível para aluguel.
        /// </summary>
        Disponível,

        /// <summary>
        /// Moto alugada.
        /// </summary>
        Alugada,

        /// <summary>
        /// Moto em manutenção.
        /// </summary>
        Manutenção
    }
}
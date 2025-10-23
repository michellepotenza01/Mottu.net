using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Models.ML
{
    [SwaggerSchema("Dados de entrada para predição de manutenção")]
    public class MotoManutencaoInput
    {
        [SwaggerSchema("Placa da moto", Format = "string")]
        public string Placa { get; set; } = string.Empty;

        [SwaggerSchema("Quilometragem atual", Format = "int32")]
        public int Quilometragem { get; set; }

        [SwaggerSchema("Quantidade de revisões realizadas", Format = "int32")]
        public int QuantidadeRevisoes { get; set; }

        [SwaggerSchema("Dias desde a última revisão", Format = "int32")]
        public int DiasDesdeUltimaRevisao { get; set; }

        [SwaggerSchema("Setor de conservação (codificado: 0=Bom, 1=Intermediario, 2=Ruim)", Format = "int32")]
        public int SetorEncoded { get; set; }
        public bool PrecisaManutencao { get; internal set; }
    }
}
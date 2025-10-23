using MottuApi.Models;
using MottuApi.Models.ML;
using Microsoft.ML;
using MottuApi.Enums;
using Microsoft.Extensions.Logging;

namespace MottuApi.Services
{
    public class MotoPredictionService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly ILogger<MotoPredictionService> _logger;

        public MotoPredictionService(ILogger<MotoPredictionService> logger)
        {
            _logger = logger;
            _mlContext = new MLContext(seed: 0);
            
            _logger.LogInformation("Inicializando modelo ML.NET...");

            try
            {
                var trainingData = new[]
                {
                    new MotoManutencaoInput 
                    { 
                        Quilometragem = 5000, 
                        QuantidadeRevisoes = 2, 
                        DiasDesdeUltimaRevisao = 30, 
                        SetorEncoded = 0, 
                        PrecisaManutencao = false 
                    },
                    new MotoManutencaoInput 
                    { 
                        Quilometragem = 15000, 
                        QuantidadeRevisoes = 1, 
                        DiasDesdeUltimaRevisao = 180, 
                        SetorEncoded = 0, 
                        PrecisaManutencao = true 
                    },
                    new MotoManutencaoInput 
                    { 
                        Quilometragem = 8000, 
                        QuantidadeRevisoes = 3, 
                        DiasDesdeUltimaRevisao = 60, 
                        SetorEncoded = 1, 
                        PrecisaManutencao = false 
                    },
                    new MotoManutencaoInput 
                    { 
                        Quilometragem = 20000, 
                        QuantidadeRevisoes = 0, 
                        DiasDesdeUltimaRevisao = 365, 
                        SetorEncoded = 2, 
                        PrecisaManutencao = true 
                    },
                    new MotoManutencaoInput 
                    { 
                        Quilometragem = 3000, 
                        QuantidadeRevisoes = 1, 
                        DiasDesdeUltimaRevisao = 90, 
                        SetorEncoded = 0, 
                        PrecisaManutencao = false 
                    },
                    new MotoManutencaoInput 
                    { 
                        Quilometragem = 12000, 
                        QuantidadeRevisoes = 2, 
                        DiasDesdeUltimaRevisao = 120, 
                        SetorEncoded = 1, 
                        PrecisaManutencao = true 
                    }
                };

                var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                // ✅ CORREÇÃO: Pipeline com nomes EXATOS das propriedades
                var pipeline = _mlContext.Transforms.Concatenate("Features", 
                        nameof(MotoManutencaoInput.Quilometragem),
                        nameof(MotoManutencaoInput.QuantidadeRevisoes), 
                        nameof(MotoManutencaoInput.DiasDesdeUltimaRevisao),
                        nameof(MotoManutencaoInput.SetorEncoded))
                    .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                    .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                        labelColumnName: nameof(MotoManutencaoInput.PrecisaManutencao),
                        featureColumnName: "Features"));

                _model = pipeline.Fit(dataView);
                _logger.LogInformation("Modelo ML.NET treinado e inicializado com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao treinar modelo ML.NET. Usando fallback.");
                _model = null; // Modelo vazio - usará fallback
            }
        }

        public MotoManutencaoPrediction PredictManutencao(Moto moto)
        {
            if (moto is null)
                throw new ArgumentNullException(nameof(moto), "Moto não pode ser nula");

            try
            {
                _logger.LogDebug("Iniciando predição ML para moto {Placa}", moto.Placa);
                
                // Se o modelo não foi treinado, usa fallback
                if (_model == null)
                {
                    _logger.LogWarning("Modelo ML não disponível. Usando fallback.");
                    return CreateFallbackPrediction(moto);
                }

                var input = CreateInputFromMoto(moto);
                var predictionEngine = _mlContext.Model.CreatePredictionEngine<MotoManutencaoInput, MotoManutencaoPrediction>(_model);
                var prediction = predictionEngine.Predict(input);

                _logger.LogInformation("Predição concluída para moto {Placa}: PrecisaManutencao={PrecisaManutencao}, Probability={Probability}", 
                    moto.Placa, prediction.PrecisaManutencao, prediction.Probability);

                return prediction;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Erro na predição ML para moto {Placa}. Usando fallback.", moto.Placa);
                return CreateFallbackPrediction(moto);
            }
        }

        private MotoManutencaoInput CreateInputFromMoto(Moto moto)
        {
            var diasDesdeRevisao = moto.DataUltimaRevisao.HasValue 
                ? (DateTime.Now - moto.DataUltimaRevisao.Value).Days 
                : 365;

            var setorEncoded = moto.Setor switch
            {
                SetorMoto.Bom => 0,
                SetorMoto.Intermediario => 1,
                SetorMoto.Ruim => 2,
                _ => 0
            };

            return new MotoManutencaoInput
            {
                Placa = moto.Placa,
                Quilometragem = moto.Quilometragem,
                QuantidadeRevisoes = moto.QuantidadeRevisoes,
                DiasDesdeUltimaRevisao = diasDesdeRevisao,
                SetorEncoded = setorEncoded
            };
        }

        private MotoManutencaoPrediction CreateFallbackPrediction(Moto moto)
        {
            var diasDesdeRevisao = moto.DataUltimaRevisao.HasValue 
                ? (DateTime.Now - moto.DataUltimaRevisao.Value).Days 
                : 365;

            var precisaManutencao = moto.Quilometragem > 10000 || 
                                   diasDesdeRevisao > 180 || 
                                   moto.Setor == SetorMoto.Ruim;

            var probability = CalculateProbability(moto, diasDesdeRevisao);

            _logger.LogInformation("Fallback prediction para moto {Placa}: PrecisaManutencao={PrecisaManutencao}, Probability={Probability}", 
                moto.Placa, precisaManutencao, probability);

            return new MotoManutencaoPrediction
            {
                PrecisaManutencao = precisaManutencao,
                Probability = probability,
                Score = probability > 0.5f ? 1.0f : 0.0f
            };
        }

        private float CalculateProbability(Moto moto, int diasDesdeRevisao)
        {
            var probability = 0.0f;

            if (moto.Quilometragem > 15000) probability += 0.4f;
            else if (moto.Quilometragem > 10000) probability += 0.3f;
            else if (moto.Quilometragem > 5000) probability += 0.1f;

            if (diasDesdeRevisao > 365) probability += 0.35f;
            else if (diasDesdeRevisao > 180) probability += 0.25f;
            else if (diasDesdeRevisao > 90) probability += 0.15f;

            if (moto.DataUltimaRevisao == null) probability += 0.3f;

            if (moto.Setor == SetorMoto.Ruim) probability += 0.25f;
            else if (moto.Setor == SetorMoto.Intermediario) probability += 0.15f;

            if (moto.QuantidadeRevisoes == 0) probability += 0.1f;

            return Math.Min(probability, 1.0f);
        }

        public List<string> ObterFatoresInfluentes(Moto moto, MotoManutencaoPrediction prediction)
        {
            if (moto is null)
                return new List<string> { "Dados da moto não disponíveis" };

            var fatores = new List<string>();
            var diasDesdeRevisao = moto.DataUltimaRevisao.HasValue 
                ? (DateTime.Now - moto.DataUltimaRevisao.Value).Days 
                : 365;

            if (moto.Quilometragem > 10000)
                fatores.Add($"Alta quilometragem ({moto.Quilometragem:N0} km)");

            if (diasDesdeRevisao > 180)
                fatores.Add($"Muito tempo desde última revisão ({diasDesdeRevisao} dias)");

            if (moto.Setor == SetorMoto.Ruim)
                fatores.Add("Estado de conservação ruim");

            if (moto.QuantidadeRevisoes == 0 && moto.Quilometragem > 5000)
                fatores.Add("Nunca foi revisada");

            _logger.LogDebug("Fatores influentes para moto {Placa}: {Fatores}", moto.Placa, string.Join(", ", fatores));

            return fatores.Any() ? fatores : new List<string> { "Nenhum fator crítico identificado" };
        }

        public string ObterRecomendacaoManutencao(MotoManutencaoPrediction prediction)
        {
            var recomendacao = prediction.PrecisaManutencao
                ? (prediction.Probability > 0.8f 
                    ? "MANUTENÇÃO URGENTE: Agendar imediatamente"
                    : "MANUTENÇÃO RECOMENDADA: Agendar preventiva")
                : (prediction.Probability < 0.3f 
                    ? "CONDIÇÃO EXCELENTE: Manutenção não necessária"
                    : "CONDIÇÃO REGULAR: Monitorar periodicamente");

            _logger.LogDebug("Recomendação para predição: {Recomendacao}", recomendacao);

            return recomendacao;
        }
    }
}
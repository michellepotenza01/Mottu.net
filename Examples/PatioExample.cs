using MottuApi.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MottuApi.Examples
{
    public class PatioExample : IExamplesProvider<PatioDto>
    {
        public PatioDto GetExamples()
        {
            return new PatioDto
            {
                NomePatio = "Patio Centro",
                Localizacao = "Av. Paulista, 1000 - São Paulo-SP",
                VagasTotais = 50
            };
        }
    }
}
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
                NomePatio = "Patio Osasco",
                Localizacao = "Av. Bussocaba, 950 - Osasco-Sp",
                VagasTotais = 80
            };
        }
    }
}
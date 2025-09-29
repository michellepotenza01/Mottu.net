using MottuApi.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MottuApi.Examples
{
    public class FuncionarioExample : IExamplesProvider<FuncionarioDto>
    {
        public FuncionarioDto GetExamples()
        {
            return new FuncionarioDto
            {
                UsuarioFuncionario = "LuhDani",
                Nome = "Luisa Danielle",
                Senha = "Luhlu00",
                NomePatio = "Patio Centro"
            };
        }
    }
}
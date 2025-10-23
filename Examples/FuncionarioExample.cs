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
                UsuarioFuncionario = "maria.silva",
                Nome = "Maria Silva",
                Senha = "SenhaSegura123",
                NomePatio = "Patio Centro",
                Role = "Funcionario"
            };
        }
    }
}
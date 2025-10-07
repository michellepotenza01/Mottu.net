using MottuApi.DTOs;
using MottuApi3.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace MottuApi.Examples
{
    public class MotoExample : IExamplesProvider<MotoDto>
    {
        public MotoDto GetExamples()
        {
            return new MotoDto
            {
                Placa = "DBC-0987",
                Modelo = ModeloMoto.MottuSport,
                Status = StatusMoto.Disponivel,
                Setor = SetorMoto.Bom,
                NomePatio = "Patio Centro",
                UsuarioFuncionario = "michelle0ptz"
            };
        }
    }
}
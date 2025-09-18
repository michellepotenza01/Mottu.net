using MottuApi.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MottuApi.Examples
{
    public class ClienteExample : IExamplesProvider<ClienteDto>
    {
        public ClienteDto GetExamples()
        {
            return new ClienteDto
            {
                UsuarioCliente = "DanPtz",
                Nome = "Danilo Potenza",
                Senha = "Senh4111",
                MotoPlaca = "ABC-1234"
            };
        }
    }
}
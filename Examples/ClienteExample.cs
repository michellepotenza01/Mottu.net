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
                UsuarioCliente = "carlos.santos",
                Nome = "Carlos Santos",
                Senha = "MinhaSenha123",
                MotoPlaca = "XYZ-5678"
            };
        }
    }
}
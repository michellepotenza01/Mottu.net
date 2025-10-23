using MottuApi.Models.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace MottuApi.Examples
{
    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                Usuario = "joao.silva",
                Senha = "SenhaSegura123"
            };
        }
    }
}
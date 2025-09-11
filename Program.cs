using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do DbContext para usar o Oracle
builder.Services.AddDbContext<MottuDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
    options.UseOracle(connectionString);
});

// Configura��o de Reposit�rios e Servi�os
builder.Services.AddScoped<IPatioRepository, PatioRepository>();
builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

builder.Services.AddScoped<IPatioService, PatioService>();
builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Configura��o dos controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configura��o para preservar refer�ncias e evitar erros de ciclo
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Configura��o do Swagger para documenta��o da API
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mottu API",
        Version = "v1",
        Description = "API para gest�o de motos, funcion�rios, p�tios e clientes."
    });

    options.TagActionsBy(api => new[] { api.ActionDescriptor.RouteValues["controller"] });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor insira o token JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Habilita o Swagger e SwaggerUI em qualquer ambiente
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mottu API v1");
    c.RoutePrefix = string.Empty; // Swagger acess�vel em "/"
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();

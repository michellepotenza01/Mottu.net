using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Repositories;
using MottuApi.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Filters; 
using MottuApi.Examples; 

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5147");
builder.Environment.EnvironmentName = "Development";

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mottu API - Gerenciamento de Patio de Motos",
        Version = "v1",
        Description = "API RESTful para gerenciamento de patio de motos com controle de vagas, funcionarios, motos e clientes.",
        Contact = new OpenApiContact
        {
            Name = "Equipe Mottu",
            Email = "suporte@mottu.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.EnableAnnotations();

    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<PatioExample>();

var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddDbContext<MottuDbContext>(options =>
    options.UseOracle(connectionString));

builder.Services.AddScoped<MotoRepository>();
builder.Services.AddScoped<PatioRepository>();
builder.Services.AddScoped<FuncionarioRepository>();
builder.Services.AddScoped<ClienteRepository>();

builder.Services.AddScoped<MotoService>();
builder.Services.AddScoped<PatioService>();
builder.Services.AddScoped<FuncionarioService>();
builder.Services.AddScoped<ClienteService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mottu API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("==============================================");
Console.WriteLine(" MOTTU API INICIADA COM SUCESSO!");
Console.WriteLine("==============================================");
Console.WriteLine(" URL: http://localhost:5147/");
Console.WriteLine(" Swagger disponivel na URL acima");
Console.WriteLine(" Environment: Development");
Console.WriteLine("==============================================");

app.Run();
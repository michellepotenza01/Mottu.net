using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Repositories;
using MottuApi.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Configuração da URL - usando apenas uma porta para evitar conflitos
builder.WebHost.UseUrls("http://localhost:5147");
builder.Environment.EnvironmentName = "Development";

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddDbContext<MottuDbContext>(options =>
    options.UseOracle(connectionString, oracleOptions =>
    {
        oracleOptions.MigrationsAssembly("MottuApi");
    }));

builder.Services.AddScoped<PatioRepository>();
builder.Services.AddScoped<FuncionarioRepository>();
builder.Services.AddScoped<MotoRepository>();
builder.Services.AddScoped<ClienteRepository>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PatioService>();
builder.Services.AddScoped<FuncionarioService>();
builder.Services.AddScoped<MotoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<HealthService>();
builder.Services.AddScoped<MotoPredictionService>();

builder.Services.AddScoped<IDataSeederService, DataSeederService>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<MottuDbContext>(
        name: "database",
        tags: new[] { "database", "oracle" })
    .AddCheck("memory", 
        () => 
        {
            var memory = GC.GetTotalMemory(false) / 1024 / 1024;
            return memory < 500 
                ? HealthCheckResult.Healthy($"Memória OK: {memory}MB")
                : HealthCheckResult.Degraded($"Memória elevada: {memory}MB");
        },
        tags: new[] { "memory" })
    .AddCheck("api", 
        () => HealthCheckResult.Healthy("API respondendo"),
        tags: new[] { "api" });

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("x-api-version")
    );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
});

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    jwtKey = "minha_chave_super_secreta_para_desenvolvimento_32_chars!";
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "MottuAPI",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "MottuClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("FuncionarioOrAdmin", policy => policy.RequireRole("Funcionario", "Admin"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mottu API v1",
        Version = "v1",
        Description = "**API Estável** - Versão principal com todos os endpoints básicos\n\n" +
                     "**Autenticação:** GETs são públicos, POST/PUT/DELETE requerem JWT\n" +
                     "**Status:** Production Ready",
        Contact = new OpenApiContact
        {
            Name = "Equipe Mottu",
            Email = "suporte@mottu.com",
            Url = new Uri("https://mottu.com.br")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Mottu API v2",
        Version = "v2", 
        Description = "**Nova Versão** - Endpoints aprimorados com ML.NET e recursos avançados\n\n" +
                     "**Novidades:**\n" +
                     "• Predição de manutenção com Machine Learning\n" +
                     "• Estatísticas detalhadas\n" +
                     "• Health checks avançados\n" +
                     "• Performance melhorada",
        Contact = new OpenApiContact
        {
            Name = "Equipe Mottu",
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    c.EnableAnnotations();
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o esquema Bearer.\n\n" +
                     "Digite: **Bearer** {seu_token} \n\n" +
                     "Exemplo: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });

    c.TagActionsBy(api =>
    {
        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        var version = api.ActionDescriptor.EndpointMetadata
            .OfType<MapToApiVersionAttribute>()
            .FirstOrDefault()?.Versions.FirstOrDefault()?.ToString() ?? "v1";
        
        return new[] { $"{controllerName} ({version})" };
    });

    c.OrderActionsBy(api => 
    {
        var httpMethodOrder = new Dictionary<string, int>
        {
            ["GET"] = 1,
            ["POST"] = 2, 
            ["PUT"] = 3,
            ["DELETE"] = 4
        };
        
        var method = api.HttpMethod ?? "";
        return $"{httpMethodOrder.GetValueOrDefault(method, 5)}-{api.RelativePath}";
    });

    c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeederService>();
        await seeder.SeedDataAsync();
        Console.WriteLine("Data seeding executado com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro no data seeding: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mottu API v1 (Estável)");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Mottu API v2 (Nova)");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Mottu API Documentation";
        c.DisplayOperationId();
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
        c.DefaultModelsExpandDepth(2);
        c.DefaultModelExpandDepth(2);
    });
}

app.UseExceptionHandler("/error");

// ✅ CORREÇÃO: Adicionar endpoint raiz com página HTML
app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Mottu API - Sistema de Gerenciamento de Pátio de Motos</title>
    <style>
        body { 
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; 
            margin: 0; 
            padding: 20px; 
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            min-height: 100vh;
        }
        .container { 
            max-width: 800px; 
            margin: 0 auto; 
            background: rgba(255,255,255,0.1); 
            padding: 30px; 
            border-radius: 15px; 
            backdrop-filter: blur(10px);
            box-shadow: 0 8px 32px rgba(0,0,0,0.1);
        }
        h1 { 
            color: white; 
            text-align: center; 
            margin-bottom: 30px;
            font-size: 2.5em;
        }
        .card { 
            background: rgba(255,255,255,0.2); 
            padding: 20px; 
            margin: 15px 0; 
            border-radius: 10px; 
            border-left: 5px solid #4CAF50;
        }
        .btn { 
            display: inline-block; 
            padding: 12px 24px; 
            background: #4CAF50; 
            color: white; 
            text-decoration: none; 
            border-radius: 5px; 
            margin: 5px;
            transition: background 0.3s;
        }
        .btn:hover { 
            background: #45a049; 
        }
        .links { 
            text-align: center; 
            margin-top: 30px;
        }
        .feature { 
            margin: 10px 0; 
            padding: 10px; 
            background: rgba(255,255,255,0.1); 
            border-radius: 5px;
        }
    </style>
</head>
<body>
    <div class="container">
        <h1> Mottu API</h1>
        <p style="text-align: center; font-size: 1.2em;">Sistema de Gerenciamento de Pátio de Motos</p>
        
        <div class="card">
            <h2> Documentação</h2>
            <p>Acesse a documentação interativa da API para explorar todos os endpoints disponíveis.</p>
            <div class="links">
                <a href="/swagger" class="btn">Swagger UI</a>
            </div>
        </div>

        <div class="card">
            <h2> Recursos da API</h2>
            <div class="feature"><strong>Versão 1 (Estável):</strong> CRUD completo, Autenticação JWT</div>
            <div class="feature"><strong>Versão 2 (Avançada):</strong> ML.NET, Paginação, Estatísticas</div>
            <div class="feature"><strong>Health Checks:</strong> Monitoramento do sistema</div>
            <div class="feature"><strong>HATEOAS:</strong> Navegação por links</div>
        </div>

        <div class="card">
            <h2> Links Úteis</h2>
            <div class="links">
                <a href="/api" class="btn">API Info</a>
                <a href="/health" class="btn">Health Check</a>
                <a href="/swagger" class="btn">Documentação</a>
            </div>
        </div>

        <div class="card">
            <h2> Credenciais de Teste</h2>
            <p><strong>Admin:</strong> usuario=admin_principal, senha=Admin123!</p>
            <p><strong>Funcionário:</strong> usuario=funcionario_teste, senha=Func123!</p>
        </div>
    </div>
</body>
</html>
""", "text/html")).WithTags("Home");

app.MapGet("/index.html", () => Results.Redirect("/")).WithTags("Home");

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        });
        await context.Response.WriteAsync(result);
    }
});

app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/api", () => new
{
    message = "Bem-vindo à Mottu API - Sistema de Gerenciamento de Pátio de Motos",
    documentation = "/swagger",
    health = "/health",
    home = "/",
    versions = new[] { 
        new { version = "v1", description = "API Estável - Endpoints básicos" },
        new { version = "v2", description = "Nova Versão - Recursos avançados com ML.NET" }
    },
    features = new[] {
        "JWT Authentication",
        "ML.NET Integration", 
        "Health Checks",
        "API Versioning",
        "HATEOAS Links",
        "Pagination",
        "Swagger Documentation"
    },
    timestamp = DateTime.Now
}).WithTags("API Info");

Console.WriteLine("==============================================");
Console.WriteLine("MOTTU API INICIADA COM SUCESSO!");
Console.WriteLine("==============================================");
Console.WriteLine($"URL Principal: http://localhost:5147");
Console.WriteLine($"Página HTML: http://localhost:5147/index.html");
Console.WriteLine($"Swagger: http://localhost:5147/swagger");
Console.WriteLine($"Health Check: http://localhost:5147/health");
Console.WriteLine($"API Info: http://localhost:5147/api");
Console.WriteLine("==============================================");
Console.WriteLine("CREDENCIAIS PARA TESTE:");
Console.WriteLine("Admin: usuario=admin_principal, senha=Admin123!");
Console.WriteLine("Funcionário: usuario=funcionario_teste, senha=Func123!");
Console.WriteLine("==============================================");
Console.WriteLine("VERSÕES DISPONÍVEIS:");
Console.WriteLine("   • v1 - API Estável (Produção)");
Console.WriteLine("   • v2 - Nova Versão (Recursos Avançados)");
Console.WriteLine("==============================================");
Console.WriteLine("DICAS:");
Console.WriteLine("   • Use /api/v1/... para versão estável");
Console.WriteLine("   • Use /api/v2/... para novos recursos");
Console.WriteLine("   • Obtenha token em: POST /api/v1/auth/login");
Console.WriteLine("==============================================");

await app.RunAsync();
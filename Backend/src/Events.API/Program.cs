using Events.API.BusinessLogic;
using Events.API.Data;
using Events.API.Data.Repositories;
using Events.API.Extensions;
using Events.API.Filters;
using Events.API.Interfaces;
using Events.API.Interfaces.Repositories;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var connectionString = builder.Configuration.GetConnectionString("EventsDbContext");
builder.Services.AddDbContext<EventsDbContext>(options => options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.AllowEmptyInputInBodyModelBinding = true;
    options.Filters.Add<ExceptionsFilter>();
})
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Enable CORS (Cross Origin Resource Sharing)
var  web2UIOrigins = "_enableCORS";

builder.Services.AddCors(options =>
    options.AddPolicy(name: web2UIOrigins,
        policy =>
        {
            policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader(); // Autoriser l'envoi d'un en-tête de n'importe quelle origine
        }
));

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Bearer";
}).AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";
        options.Audience = "web2Api";
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true
        };
    });

// Declare Auth policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
    options.AddPolicy("ManagerPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "manager"));
    options.AddPolicy("RequireScope", policy => policy.RequireClaim(JwtClaimTypes.Scope, "web2ApiScope"));

    options.DefaultPolicy = options.GetPolicy("RequireScope");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API - Gestionnaire d'événements",
        Description = "API pour la gestion des covoiturages",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Groupe TP2",
            Email = "grp@test.com",
            Url = new Uri("https://google.com/")
        },
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("http://www.apache.org")
        }
    });

    // AddSwagerGen options
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                TokenUrl = new Uri("https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    {"web2ApiScope", "Demo API - scope web2Api"}
                }
            }
        }
    });

    c.OperationFilter<AuthorizeCheckOperationFilter>();

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
    c.IncludeXmlComments(xmlFilePath);
});

// Ajout des services repositories au conteneur
builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
builder.Services.AddScoped<IEvenementsRepository, EvenementsRespository>();
builder.Services.AddScoped<IParticipationsRepository, ParticipationsRepository>();

// Ajout des services interfaces au conteneur
builder.Services.AddScoped<IVillesBL, VillesBL>();
builder.Services.AddScoped<ICategoriesBL, CategoriesBL>();
builder.Services.AddScoped<IEvenementsBL, EvenementsBL>();
builder.Services.AddScoped<IParticipationsBL, ParticipationsBL>();
builder.Services.AddScoped<IStatisticsBL, StatisticsBL>();

// Injection du service d'AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();//.RequireAuthorization(); // tous les controlleurs mapés doivent être protégés

// Générer les données
app.CreateDbIfNotExists();

// Execute CORS
app.UseCors(web2UIOrigins);

app.Run();

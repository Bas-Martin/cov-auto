using System.Text;
using CovAuto.API.Application.Interfaces;
using CovAuto.API.Application.Services;
using CovAuto.API.Infrastructure.Data;
using CovAuto.API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// --- CORS for Blazor WASM client ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5264", "https://localhost:7124")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- Controllers ---
builder.Services.AddControllers();

// --- Swagger / OpenAPI ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CovAuto API",
        Version = "v1",
        Description = "Voorbeeld Web API voor monteurs en werkorders. " +
                      "Demonstreert authenticatie, autorisatie, filtering, sorting, pagination en async verwerking."
    });

    // Voeg JWT Bearer token toe aan Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Voer je JWT token in: Bearer {token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});

// --- Database (SQLite) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- JWT Authenticatie ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

// --- Dependency Injection: Repositories registreren ---
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceTeamRepository, ServiceTeamRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();

// --- Dependency Injection: Services registreren ---
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IServiceTeamService, ServiceTeamService>();
builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
builder.Services.AddScoped<IReportService, ReportService>();

// --- Logging ---
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// --- Database migraties en seed data ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// --- Middleware pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CovAuto API v1");
        options.RoutePrefix = string.Empty; // Swagger op root URL
    });
}

app.UseCors("BlazorClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

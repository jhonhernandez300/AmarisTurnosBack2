using Core;
using Core.Interfaces;
using Core.Models;
using EF;
using EF.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AutoMapper;
using KontrolarCloud.Mapping;
using KontrolarCloud.Middlewares;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Core.Modelos;
using Core.Modelos.Interfaces;
using EF.Repositorios;
using EF.Servicios;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
        .AllowAnyMethod());
});


builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("EF")
    ),
    ServiceLifetime.Transient 
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBaseRepositorio<Usuario>, BaseRepositorio<Usuario>>();
builder.Services.AddScoped<ISucursalRepositorio, SucursalRepositorio>();
builder.Services.AddScoped<ITurnoRepositorio, TurnoRepositorio>();
builder.Services.AddScoped<ITurnoHistorialRepositorio, TurnoHistorialRepositorio>();

builder.Services.AddSingleton<ITurnoService>(new TurnoService(
    configuration.GetConnectionString("DefaultConnection") // cambio: ahora obtiene la cadena de conexión desde el appsettings.json
));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KontrolarCloud", Version = "v1" });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        IConfiguration configuration = builder.Configuration;

        if (configuration != null)
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        }
        else
        {
            throw new InvalidOperationException("Configuration is null.");
        }
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(
    typeof(MappingUsuario), 
    typeof(MappingSucursal),
    typeof(MappingTurnos),
    typeof(MappingTurnoHistorial)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KontrolarCloud v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowOrigins");

app.UseMiddleware<TokenValidationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapFallbackToFile("/index.html");

app.Use(async (context, next) =>
{
    // Log de la solicitud
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});



app.Run();

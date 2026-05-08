using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SL_Api_Ecommerce.Data;
using SL_Api_Ecommerce.Repository;
using SL_Api_Ecommerce.Repository.IRepository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"))
);

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024; // Tamaño máximo de la respuesta en bytes
    options.SizeLimit = 1024 * 1024 * 100; // Tamaño máximo total del caché en bytes (100 MB)
    options.UseCaseSensitivePaths = true; // Considerar mayúsculas y minúsculas en las rutas
}
);

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(cfg => {
    // Configuración de AutoMapper si es necesaria
}, typeof(Program));

var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; //desactiva https solo para desarrollo en prod es true
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretKey")),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        options.Authority = "http://localhost:8080/realms/master-realm";
        options.Audience = "ecommerce-api";
    });

builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default10", new CacheProfile
    {
        Duration = 10 // Duración del caché en segundos
    });
    options.CacheProfiles.Add("Default20", new CacheProfile
    {
        Duration = 20 // Duración del caché en segundos
    });
}
);

builder.Services.AddEndpointsApiExplorer(); // Necesario para Swagger
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAEspecificOrigin", builder =>
    {
        builder.WithOrigins("*")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAEspecificOrigin");

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
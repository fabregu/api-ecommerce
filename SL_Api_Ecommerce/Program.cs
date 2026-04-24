using Microsoft.EntityFrameworkCore;
using SL_Api_Ecommerce.Data;
using SL_Api_Ecommerce.Repository;
using SL_Api_Ecommerce.Repository.IRepository;
using Microsoft.Extensions.DependencyInjection; // Asegúrate de tener este using
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"))
);
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddAutoMapper(cfg => {
    // Configuración de AutoMapper si es necesaria
}, typeof(Program));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); // Necesario para Swagger
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Models;
using PedalPricerServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PedalPricerDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
builder.Services.AddSingleton<FileService>();

var CorsPolicy = "_CorsPolicy";
builder.Services.AddCors(p => p.AddPolicy(CorsPolicy, builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Db;
using PedalPricerServer.Models;
using PedalPricerServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var fileService = new FileService();

string bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME") ?? throw new Exception("Bucket name not specified");
string filename = Environment.GetEnvironmentVariable("SQLITE_FILENAME") ?? throw new Exception("Sqlite file name not specified");
string localSqlitePath = Path.Combine(Path.GetTempPath(), filename);
if (!File.Exists(localSqlitePath))
{
    using var sqliteStream = await fileService.ReadFile(MediaFolder.Sqlite, filename);
    using var fileStream = new FileStream(localSqlitePath, FileMode.Create, FileAccess.Write);
    await sqliteStream.CopyToAsync(fileStream);
}

builder.Services.AddDbContext<PedalPricerDbContext>(options => options.UseSqlite($"Data Source={localSqlitePath}"));
builder.Services.AddSingleton<FileService>();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

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

app.MapGet("ping", () =>
{
    return "pong";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

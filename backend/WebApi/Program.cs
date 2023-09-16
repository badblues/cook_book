using Domain;

using Persistence;

using Serilog;

using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
Log.Information("Server started");

string? storagePath =
    builder.Configuration.GetSection("Storage:StoragePath").Value
    ?? throw new NullReferenceException("Storage path isn't configured");
builder.Services.AddScoped<IRepository<Recipe>>(provider =>
{
    return new StorageRepository(storagePath);
});
builder.Services.AddScoped<RecipeConverter>(provider => new RecipeConverter(storagePath));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
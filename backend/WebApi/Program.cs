using CookBook.Domain;
using CookBook.Persistence;
using CookBook.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

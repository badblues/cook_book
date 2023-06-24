using CookBook.Domain;
using CookBook.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//TODO must be a better way of doing this
string? storagePath =
    builder.Configuration.GetSection("Storage:StoragePath").Value
    ?? throw new NullReferenceException();
builder.Services.AddScoped<IRepository<Recipe>>(provider => 
{
    return new StorageRepository(storagePath);
});

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

using FluentValidation;
using FruitsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registra il DbContext con il provider In-Memory
builder.Services.AddDbContext<FruitDbContext>(opt =>
    opt.UseInMemoryDatabase("FruitDB"));

// Registra i servizi per Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Registra il validatore nel contenitore DI
builder.Services.AddScoped<IValidator<FruitModel>, FruitValidator>();


var app = builder.Build();

// Abilita Swagger solo in ambiente di sviluppo
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");


// GET /fruits — restituisce tutti i frutti
app.MapGet("/fruits", async (FruitDbContext db) =>
    await db.Fruits.ToListAsync());


// GET /fruits/{id} — restituisce un frutto per ID
app.MapGet("/fruits/{id}", async (int id, FruitDbContext db) =>
    await db.Fruits.FindAsync(id) is FruitModel fruit
        ? Results.Ok(fruit)
        : Results.NotFound());


// POST /fruits — crea un nuovo frutto
app.MapPost("/fruits", async (FruitModel fruit, IValidator<FruitModel> validator, FruitDbContext db) =>
{

    // Esegui la validazione sul modello ricevuto
    var result = await validator.ValidateAsync(fruit);

    // Se la validazione fallisce, restituisce 400 con i dettagli degli errori
    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());

    db.Fruits.Add(fruit);

    await db.SaveChangesAsync();
    return Results.Created($"/fruits/{fruit.id}", fruit);

});

// PUT /fruits/{id} — aggiorna un frutto esistente
app.MapPut("/fruits/{id}", async (int id, FruitModel input, IValidator <FruitModel> validator,  FruitDbContext db) =>
{
    // Esegui la validazione sull'input ricevuto
    var result = await validator.ValidateAsync(input);
    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());

    var fruit = await db.Fruits.FindAsync(id);
    if (fruit is null) return Results.NotFound();

    fruit.name = input.name;
    fruit.instock = input.instock;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// DELETE /fruits/{id} — elimina un frutto
app.MapDelete("/fruits/{id}", async (int id, FruitDbContext db) =>
{
    var fruit = await db.Fruits.FindAsync(id);

    if (fruit is null) return Results.NotFound();

    db.Fruits.Remove(fruit);
    await db.SaveChangesAsync();
    return Results.Ok(fruit);
});








app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

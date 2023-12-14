using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var fruits = new List<string>()
{
    "Banana",
    "Apple",
    "Strawberry"
};

//minimal api endpoint to get all categories
app.MapGet("/fruits", () =>
   {
       return Results.Ok(fruits);
   });

//minimal api en

app.Run();
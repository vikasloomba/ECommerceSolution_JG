using ECommerceApi.Adapters;
using ECommerceApi.Domain;
using ECommerceApi.Models;
using MongoDB.Bson.Serialization.Conventions;

var builder = WebApplication.CreateBuilder(args);

var conventionPack = new ConventionPack
{
    new CamelCaseElementNameConvention(),
    new IgnoreExtraElementsConvention(true)
};
ConventionRegistry.Register("defaults", conventionPack, t => true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Adapter Services.
var mongoConnectionString = builder.Configuration.GetConnectionString("mongo");
builder.Services.AddSingleton<MongoDbOrdersAdapter>(sp =>
{
    return new MongoDbOrdersAdapter(mongoConnectionString);
});


builder.Services.AddScoped<OrderProcessor>(); // lazy


// This is setting up the middleware and request/response pipeline
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

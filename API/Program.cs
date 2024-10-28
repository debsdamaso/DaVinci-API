using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using API.Repositories; 
using DaVinci.Services; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuração do MongoDB
var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDB");
var mongoClientSettings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);

// Set the ServerApi field of the settings object to set the version of the Stable API on the client
mongoClientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);

// Criação do cliente MongoDB
var client = new MongoClient(mongoClientSettings);

// Send a ping to confirm a successful connection
try
{
    var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
    Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to connect to MongoDB: {ex.Message}");
}

// Adiciona o cliente MongoDB ao contêiner de serviços
builder.Services.AddSingleton<IMongoClient>(client);

// Registre o ProductService
builder.Services.AddScoped<ProductService>();

// Registre o repositório, se você tiver um
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DaVinci API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DaVinci API v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();

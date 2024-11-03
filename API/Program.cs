using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using API.Repositories;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddControllers();

// Configuração do MongoDB
var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDB");
var mongoClientSettings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);
mongoClientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(mongoClientSettings);

// Testa a conexão com o MongoDB
try
{
    var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
    Console.WriteLine("Conexão bem-sucedida ao MongoDB!");
}
catch (Exception ex)
{
    Console.WriteLine($"Falha na conexão ao MongoDB: {ex.Message}");
}

// Registro do cliente MongoDB
builder.Services.AddSingleton<IMongoClient>(client);

// Registro do banco de dados
builder.Services.AddSingleton<IMongoDatabase>(provider =>
{
    var mongoClient = provider.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase("DaVinciDB"); // Substitua pelo nome do seu banco de dados
});

// Adiciona serviços e repositórios
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CostumerService>();
builder.Services.AddScoped<FeedbackService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<ICostumerRepository, CostumerRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>(); // Registrar o IPurchaseRepository
builder.Services.AddScoped<FeedbackReminderService>();
builder.Services.AddScoped<IEmailService, EmailService>(); // A implementação do IEmailService já estava correta

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DaVinci API", Version = "v1" });
});

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
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

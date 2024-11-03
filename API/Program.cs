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
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    return new MongoClient(mongoDbConnectionString);
});

// Registro do banco de dados MongoDB como serviço
builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("DaVinciDB");
});

// Registro dos repositórios e serviços
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<ICostumerRepository, CostumerRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<FeedbackReminderService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();

// saber o log do email
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Adiciona Swagger para documentação
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DaVinci API", Version = "v1" });
});

var app = builder.Build();

// Configurações do app
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DaVinci API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class Product
    {
        [BsonId] // Identificador único do documento
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")] // Nome do produto
        public string Name { get; set; }

        [BsonElement("description")] // Descrição do produto
        public string Description { get; set; }

        [BsonElement("price")] // Preço do produto
        public decimal Price { get; set; }

        [BsonElement("createdAt")] // Data de criação
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Definindo data padrão

        // Construtor para garantir que as propriedades sejam inicializadas
        public Product(string id, string name, string description, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}

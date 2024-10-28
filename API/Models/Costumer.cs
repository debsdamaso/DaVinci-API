using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DaVinci.Models
{
    public class Costumer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        // Construtor para garantir que as propriedades sejam inicializadas
        public Costumer(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}

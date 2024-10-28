using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DaVinci.Models
{
    public class Feedback
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; }

        [BsonElement("sentiment")]
        public string Sentiment { get; set; }

        // Construtor para garantir que as propriedades sejam inicializadas
        public Feedback(string id, string customerId, string productId, string comment, string sentiment)
        {
            Id = id;
            CustomerId = customerId;
            ProductId = productId;
            Comment = comment;
            Sentiment = sentiment;
        }
    }
}

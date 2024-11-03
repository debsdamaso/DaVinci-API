using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class FeedbackCreateDto
    {
        [BsonElement("customerId")]
        public string? CustomerId { get; set; }

        [BsonElement("productId")]
        public string? ProductId { get; set; }

        [BsonElement("comment")]
        public string? Comment { get; set; }
    }
}

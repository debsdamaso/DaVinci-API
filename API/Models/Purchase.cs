using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace API.Models
{
    public class Purchase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("customerId")]
        public required string CustomerId { get; set; }

        [BsonElement("productId")]
        public required string ProductId { get; set; }

        [BsonElement("purchaseDate")]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [BsonElement("feedbackDeixado")]
        public bool FeedbackDeixado { get; set; } = false; // Inicialmente como falso

        // Propriedade adicionada para o e-mail do cliente, se necessário
        [BsonElement("customerEmail")]
        public string? CustomerEmail { get; set; }

        // Referência ao ID do Feedback associado, se houver
        [BsonElement("feedbackId")]
        public string? FeedbackId { get; set; }

        // Método para verificar se um feedback foi associado
        public bool HasFeedback => FeedbackId != null;
    }
}

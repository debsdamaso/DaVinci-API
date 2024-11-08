﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class Feedback
    {
        [BsonId] // Identificador único do documento
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("customerId")] // ID do cliente
        public string? CustomerId { get; set; }

        [BsonElement("productId")] // ID do produto
        public required string ProductId { get; set; }

        [BsonElement("comment")] // Comentário do feedback
        public string? Comment { get; set; }

        [BsonElement("sentiment")] // Sentimento previsto
        public string? Sentiment { get; set; } // "Positive" ou "Negative"

        [BsonElement("sentimentScore")] // Pontuação da previsão
        public float SentimentScore { get; set; }
    }
}

namespace API.Models
{
    public class FeedbackDto
    {
        public required string Id { get; set; } // ID do feedback
        public string? CustomerId { get; set; } // ID do cliente
        public string? ProductId { get; set; } // ID do produto
        public string? Comment { get; set; } // Comentário do feedback
        public string? Sentiment { get; set; } // Sentimento previsto
    }
}

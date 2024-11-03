using Microsoft.ML.Data;

namespace API.Models
{
    public class SentimentData
    {
        [LoadColumn(0)]
        public string? Comment { get; set; }

        [LoadColumn(1)]
        public float Score { get; set; }
    }
}

using Microsoft.ML.Data;

namespace DaVinci.Models
{
    public class SentimentData
    {
        [LoadColumn(0)]
        public string? Comment { get; set; }

        [LoadColumn(1)]
        public float Score { get; set; } 
    }
}

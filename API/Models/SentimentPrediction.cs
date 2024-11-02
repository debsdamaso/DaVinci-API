using Microsoft.ML.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DaVinci.Models
{
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; } 
    }
}

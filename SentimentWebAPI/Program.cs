using Microsoft.ML.Data;
using Microsoft.Extensions.ML;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPredictionEnginePool<ModelInput, ModelOutput>()
    .FromFile(modelName: "SentimentAnalysisModel", filePath: "sentiment_model.zip", watchForChanges: true);

var app = builder.Build();

var predictionHandler =
    async (PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool, ModelInput input) =>
        await Task.FromResult(predictionEnginePool.Predict(modelName: "SentimentAnalysisModel", input));

app.MapPost("/predict", predictionHandler);

app.Run();

public class ModelInput
{
    public string SentimentText;
}

public class ModelOutput
{
    [ColumnName("PredictedLabel")]
    public bool Sentiment { get; set; }

    public float Probability { get; set; }

    public float Score { get; set; }
}
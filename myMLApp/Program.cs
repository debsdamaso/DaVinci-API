using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace MyMLApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new ML context for ML.NET operations
            var mlContext = new MLContext();

            // Load your data and train your model here
            // For example, let's assume you have a trained model called 'trainedModel'
            ITransformer trainedModel = null; // Replace with your actual trained model

            // Define the relative path where you want to save the trained model
            string modelRelativePath = "MLModels/trainedModel.zip";

            // Get the absolute path to save the model
            string modelPath = Path.Combine(Environment.CurrentDirectory, modelRelativePath);

            // Save the trained model to a .zip file
            using (var fileStream = new FileStream(modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mlContext.Model.Save(trainedModel, null, fileStream);
            }

            Console.WriteLine($"Model saved to: {modelPath}");
        }
    }
}
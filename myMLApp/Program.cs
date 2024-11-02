using MyMLApp;
// Adicione uma frase
var sampleData = new MLModel.ModelInput()
{
    Col0 = "material de baixa qualidade parece barato"
};

// Carrega o modelo e prevê uma saída de dado de exemplo
var result = MLModel.Predict(sampleData);

// Se a predicao 1, sentimento "Positivo"; se nao, sentimento "Negativo"
var sentiment = result.PredictedLabel == 1 ? "Positivo" : "Negativo";
Console.WriteLine($"Text: {sampleData.Col0}\nSentiment: {sentiment}");
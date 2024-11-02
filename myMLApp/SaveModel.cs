using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.ML;

namespace MyMLApp
{
    public class SaveModel
    {
        public static void SaveTrainedModel(ITransformer model, IDataView data, string modelPath)
        {
            var mlContext = new MLContext();

            // Pull the data schema from the IDataView used for training the model
            DataViewSchema dataViewSchema = data.Schema;

            using (var fs = new FileStream(modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mlContext.Model.Save(model, dataViewSchema, fs);
            }
        }
    }
}
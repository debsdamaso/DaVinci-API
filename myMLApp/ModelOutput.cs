using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myMLApp
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Sentiment { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}

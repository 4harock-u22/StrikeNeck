using Microsoft.ML;
using NewFLD;
using System.Reflection;
namespace NewFLD
{
    // All the code in this file is included in all platforms.
    public class Class1
    {
        public static void Main()
        {
            //Load sample data
            var sampleData = new MLModel1.ModelInput()
            {
                N_LEar_X = -19.03027F,
                N_LEar_Y = 22.9809F,
                REar_REye_X = -18.55717F,
                REar_REye_Y = 7.549721F,
                LEye_LEar_X = -32.21269F,
                LEye_LEar_Y = 10.91969F,
                REye_N_X = -19.78388F,
                REye_N_Y = -17.33004F,
                N_LEye_X = -19.03027F,
                N__Eye_Y = 22.9809F,
            };

            //Load model and predict output
            var result = MLModel1.Predict(sampleData);


            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("posenet");
        }
    }
}
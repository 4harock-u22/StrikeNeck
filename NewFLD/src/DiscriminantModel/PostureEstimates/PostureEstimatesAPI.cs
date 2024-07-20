using Microsoft.ML;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp;

namespace DiscriminantModel.PostureEstimates
{
    public class PostureEstimatesAPI
    {
        //predict
        public async Task<Dictionary<KeyPointType, TwoDPoint>> Predict(Image<Rgb24> predictedImage)
        {
            var dataPretreatment = new PostureEstimatesDataPretreatment();

            var inputList = dataPretreatment.convertImageToInputList(predictedImage);
            var session = dataPretreatment.buildSession();

            var output = session.Run(inputList);

            var dataPostprocessing = new PostureEstimatesDataPostProcessor();
            var keyPoints = dataPostprocessing.calcKeyPointsFromResults(output);

            return keyPoints;
        }
    }
}

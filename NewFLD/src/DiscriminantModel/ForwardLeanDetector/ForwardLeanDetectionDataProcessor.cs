using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using NewFLD;
using DiscriminantModel.PostureEstimates;

namespace DiscriminantModel.ForwardLeanDetector
{
    internal class ForwardLeanDetectionPretreatment
    {

        internal MLModel1.ModelInput buildModelInput(Dictionary<KeyPointType, TwoDPoint> jointPositions)
        {
            var distanceBetweenKeyPoints = calcDistanceBetweenKeyPoints(jointPositions);

            var modelInput = new MLModel1.ModelInput()
            {
                N_LEar_X = distanceBetweenKeyPoints[InputType.N_LEar_X],
                N_LEar_Y = distanceBetweenKeyPoints[InputType.N_LEar_Y],
                REar_REye_X = distanceBetweenKeyPoints[InputType.REar_REye_X],
                REar_REye_Y = distanceBetweenKeyPoints[InputType.REar_REye_Y],
                LEye_LEar_X = distanceBetweenKeyPoints[InputType.LEye_LEar_X],
                LEye_LEar_Y = distanceBetweenKeyPoints[InputType.LEye_LEar_Y],
                REye_N_X = distanceBetweenKeyPoints[InputType.REye_N_X],
                REye_N_Y = distanceBetweenKeyPoints[InputType.REye_N_Y],
                N_LEye_X = distanceBetweenKeyPoints[InputType.N_LEye_X],
                N__Eye_Y = distanceBetweenKeyPoints[InputType.N_LEye_Y],
            };

            return modelInput;
        }
        private Dictionary<InputType, float> calcDistanceBetweenKeyPoints(Dictionary<KeyPointType, TwoDPoint> keyPoints)
        {
            Dictionary<InputType, float> modelInput = new Dictionary<InputType, float>();

            var n_LEarX = keyPoints[KeyPointType.Nose].x - keyPoints[KeyPointType.LeftEar].x;
            var n_LEarY = keyPoints[KeyPointType.Nose].y - keyPoints[KeyPointType.LeftEar].y;

            var rEar_rEyeX = keyPoints[KeyPointType.RightEar].x - keyPoints[KeyPointType.RightEye].x;
            var rEar_rEyeY = keyPoints[KeyPointType.RightEar].y - keyPoints[KeyPointType.RightEye].y;

            var lEye_lEarX = keyPoints[KeyPointType.LeftEye].x - keyPoints[KeyPointType.LeftEar].x;
            var lEye_lEarY = keyPoints[KeyPointType.LeftEye].y - keyPoints[KeyPointType.LeftEar].y;

            var rEye_nX = keyPoints[KeyPointType.RightEye].x - keyPoints[KeyPointType.Nose].x;
            var rEye_nY = keyPoints[KeyPointType.RightEye].y - keyPoints[KeyPointType.Nose].y;

            var n_lEyeX = keyPoints[KeyPointType.Nose].x - keyPoints[KeyPointType.LeftEye].x;
            var n_lEyeY = keyPoints[KeyPointType.Nose].y - keyPoints[KeyPointType.LeftEye].y;

            modelInput.Add(InputType.N_LEar_X, n_LEarX);
            modelInput.Add(InputType.N_LEar_Y, n_LEarY);
            modelInput.Add(InputType.REar_REye_X, rEar_rEyeX);
            modelInput.Add(InputType.REar_REye_Y, rEar_rEyeY);
            modelInput.Add(InputType.LEye_LEar_X, lEye_lEarX);
            modelInput.Add(InputType.LEye_LEar_Y, lEye_lEarY);
            modelInput.Add(InputType.REye_N_X, rEye_nX);
            modelInput.Add(InputType.REye_N_Y, rEye_nY);
            modelInput.Add(InputType.N_LEye_X, n_lEyeX);
            modelInput.Add(InputType.N_LEye_Y, n_lEyeY);

            return modelInput;
        }
    }

    internal class ForwardLeanDetectionPostProcessor
    {
        internal bool isInForwardLean(MLModel1.ModelOutput modelOutput)
        {
            var resoruce = new ForwardLeanDetectionResource();
            var bias = resoruce.fldBias;

            var result = modelOutput.Score[0] + bias;

            return 0.5 < result;
        }
    }
}

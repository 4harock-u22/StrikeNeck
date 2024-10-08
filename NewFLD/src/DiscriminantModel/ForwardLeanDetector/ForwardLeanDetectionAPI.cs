﻿using DiscriminantModel.PostureEstimates;
using NewFLD;


namespace DiscriminantModel.ForwardLeanDetector
{
    class ForwardLeanDetectionAPI
    {
        public async Task<bool> Predict(Dictionary<KeyPointType, TwoDPoint> jointPositions, double bias)
        {
            var preprocessor = new ForwardLeanDetectionPretreatment();
            var postProcessor = new ForwardLeanDetectionPostProcessor();
            var input = preprocessor.buildModelInput(jointPositions);
            var output = MLModel1.Predict(input);
            var result = postProcessor.isInForwardLean(output, bias);

            return result;
        }

        public async Task Retrain(DirectoryInfo currentPostureData, DirectoryInfo forwardPostureData)
        {
            var trainer = new ForwardLeanDetectionTrainer();
            await trainer.Retrain(currentPostureData, forwardPostureData);
        }
    }
}

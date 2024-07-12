using DiscriminantModel.PostureEstimates;
using NewFLD;


namespace DiscriminantModel.ForwardLeanDetector
{
    class ForwardLeanDetectionAPI
    {
        public bool Predict(Dictionary<KeyPointType, TwoDPoint> jointPositions)
        {
            var preprocessor = new ForwardLeanDetectionPretreatment();
            var postProcessor = new ForwardLeanDetectionPostProcessor();
            var input = preprocessor.buildModelInput(jointPositions);
            var output = MLModel1.Predict(input);
            var result = postProcessor.isInForwardLean(output);

            return result;
        }

        public async void Retrain(DirectoryInfo currentPostureData, DirectoryInfo forwardPostureData)
        {
            var trainer = new ForwardLeanDetectionTrainer();
            trainer.Retrain(currentPostureData, forwardPostureData);
        }
    }
}

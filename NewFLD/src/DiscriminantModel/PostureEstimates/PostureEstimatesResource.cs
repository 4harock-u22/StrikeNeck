using System.Reflection;

namespace DiscriminantModel.PostureEstimates
{
    internal class PostureEstimatesResource
    {
        internal const int predictedImagesHeight = 257;
        internal const int predictedImagesWidth = 257;

        static internal readonly float[] mean = new[] { 0.485f, 0.456f, 0.406f };
        static internal readonly float[] stddev = new[] { 0.229f, 0.224f, 0.225f };

        //tensorSize[3]はRGBの3チャンネル
        static internal readonly int[] inputTensorSize = new[] { 1, predictedImagesHeight, predictedImagesWidth, 3 };

        internal readonly StreamReader posenetStreamReader = new StreamReader(
                                                                    Assembly.GetExecutingAssembly()
                                                                    .GetManifestResourceStream("posenet")
                                                                );

        internal const int heatmapsWidth = 9;
        internal const int heatmapsHeight = 9;


        static internal readonly int numberOfKeyPoints = Enum.GetValues<KeyPointType>().Length;

        internal const int intervalOfHeatmap = 32;

    }

    public enum KeyPointType
    {
        Nose,
        LeftEye,
        RightEye,
        LeftEar,
        RightEar,
        LeftShoulder,
        RightShoulder,
        LeftElbow,
        RightElbow,
        LeftWrist,
        RightWrist,
        LeftHip,
        RightHip,
        LeftKnee,
        RightKnee,
        LeftAnkle,
        RightAnkle
    }
}

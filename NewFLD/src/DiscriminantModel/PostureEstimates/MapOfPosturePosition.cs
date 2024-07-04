using System.Collections.Generic;

namespace DiscriminantModel.PostureEstimates
{
    public class MapOfPosturePosition
    {
        private readonly Dictionary<KeyPointType, TwoDPoint> _map;

        public MapOfPosturePosition()
        {
            _map = new Dictionary<KeyPointType, TwoDPoint>();

            foreach (var keyPointName in Enum.GetValues(typeof(KeyPointType)))
            {
                _map.Add((KeyPointType)keyPointName, new TwoDPoint(0, 0));
            }
        }

        public MapOfPosturePosition(Dictionary<KeyPointType, TwoDPoint> map)
        {
            _map = map;
        }

        public TwoDPoint GetPosition(KeyPointType keyPointType)
        {
            return _map[keyPointType];
        }
    }
}

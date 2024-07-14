using UnityEngine;

namespace PushItOut.Configs.Source
{
    [CreateAssetMenu(fileName = "IceBallConfig", menuName = "Configs/IceBallConfig")]
    public class IceBallConfigSource : BallConfigSource
    {
        public float SlowdownPercentage;
        public float TimeSlowdown;
    }
}

using UnityEngine;

namespace Rebel_Mage.Configs.Source
{
    [CreateAssetMenu(fileName = "IceBallConfig", menuName = "Configs/IceBallConfig")]
    public class IceBallConfigSource : BallConfigSource
    {
        public float SlowdownPercentage;
        public float TimeSlowdown;
    }
}

using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Configs
{
    [CreateAssetMenu(fileName = "DashConfig", menuName = "Configs/Dash Config")]
    public class DashConfigSource : SpellConfig
    {
        [Space]
        public float MoveDistance;
    }
}

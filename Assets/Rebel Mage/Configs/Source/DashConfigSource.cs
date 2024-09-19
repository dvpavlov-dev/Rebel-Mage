using Rebel_Mage.Spell_system.Configs;
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

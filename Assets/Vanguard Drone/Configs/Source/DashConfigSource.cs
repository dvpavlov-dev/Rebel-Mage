using PushItOut.Spell_system.Configs;
using UnityEngine;

namespace PushItOut.Configs
{
    [CreateAssetMenu(fileName = "DashConfig", menuName = "Configs/Dash Config")]
    public class DashConfigSource : SpellConfig
    {
        public float MoveDistance;
    }
}

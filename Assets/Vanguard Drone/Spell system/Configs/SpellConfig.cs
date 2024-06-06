using UnityEngine;

namespace PushItOut.Spell_system.Configs
{
    public class SpellConfig : ScriptableObject
    {
        [Header("Assets/Resources/Path")]
        public string Path;

        [Space]
        public string SpellName;
        public GameObject SpellPrefab;
        public float Cooldown;
        public Sprite SpellImage;
    }
}

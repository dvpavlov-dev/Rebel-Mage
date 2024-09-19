using UnityEngine;

namespace Rebel_Mage.Spell_system.Configs
{
    public class SpellConfig : ScriptableObject
    {
        public string SpellName;
        public GameObject SpellPrefab;
        public float Cooldown;
        public Sprite SpellImage;
        public int OpenAfterRound;
        public string AnimationName;
        public float AnimationTime;
    }
}

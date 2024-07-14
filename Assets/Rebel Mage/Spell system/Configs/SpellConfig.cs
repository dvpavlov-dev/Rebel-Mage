using UnityEngine;

namespace PushItOut.Spell_system.Configs
{
    public class SpellConfig : ScriptableObject
    {
        public string SpellName;
        public GameObject SpellPrefab;
        public float Cooldown;
        public Sprite SpellImage;
        public int OpenAfterRound;
    }
}

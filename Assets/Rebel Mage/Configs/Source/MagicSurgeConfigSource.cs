using UnityEngine;

namespace Rebel_Mage.Configs.Source
{
    [CreateAssetMenu(fileName = "MagicSurgeConfig", menuName = "Configs/Magic Surge config")]
    public class MagicSurgeConfigSource : AttackSpellConfig
    {
        public GameObject ChargeEffect;
        public AudioClip ChargeSound;
    }
}

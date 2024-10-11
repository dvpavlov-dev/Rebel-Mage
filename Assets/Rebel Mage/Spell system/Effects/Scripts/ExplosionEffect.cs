using UnityEngine;
using UnityEngine.Serialization;

namespace Rebel_Mage.Spell_system.Effects
{
    public class ExplosionEffect : MonoBehaviour
    {
        [FormerlySerializedAs("IceExplosion")]
        public ParticleSystem Explosion;
        
        void Start()
        {
            Destroy(gameObject, Explosion.main.duration);
        }
    }
}

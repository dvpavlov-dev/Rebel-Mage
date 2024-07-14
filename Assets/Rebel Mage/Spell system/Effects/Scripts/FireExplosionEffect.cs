using UnityEngine;

namespace Vanguard_Drone.Spell_system.Effects
{
    public class FireExplosionEffect : MonoBehaviour
    {
        public ParticleSystem ExplosionEffect;
        
        void Start()
        {
            Destroy(gameObject, ExplosionEffect.main.duration);
        }
    }
}

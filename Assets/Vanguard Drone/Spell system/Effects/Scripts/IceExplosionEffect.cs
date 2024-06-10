using UnityEngine;

namespace Vanguard_Drone.Spell_system.Effects
{
    public class IceExplosionEffect : MonoBehaviour
    {
        public ParticleSystem IceExplosion;
        
        void Start()
        {
            Destroy(gameObject, IceExplosion.main.duration);
        }
    }
}

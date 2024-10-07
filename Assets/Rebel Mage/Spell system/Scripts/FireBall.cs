using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class FireBall : BallSpell
    {
        public FireBallConfigSource FireBallConfig;
        public GameObject ExplosionEffectPref;

        protected override SpellConfig Config => FireBallConfig;
        
        protected override void ImpactOnObject(GameObject hitObject)
        {
            base.ImpactOnObject(hitObject);

            if (hitObject.GetComponent<Rigidbody>() != null) // impact on dynamic object (some objects with physics)
            {
                hitObject.GetComponent<Rigidbody>().AddExplosionForce(FireBallConfig.ExplosionForce, transform.position, FireBallConfig.ExplosionRadius);
            }

            if (hitObject.GetComponent<IImpact>() is {} impactSystem) // impact on actors (player or enemy)
            {
                impactSystem.ExplosionImpact(transform.position, FireBallConfig.ExplosionRadius, FireBallConfig.ExplosionForce);
            }
        }

        protected override void OnDestroyProjectile()
        {
            base.OnDestroyProjectile();
            
            Instantiate(ExplosionEffectPref, transform.position, transform.rotation);
        }
    }
}

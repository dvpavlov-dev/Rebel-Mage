using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class FireBall : BallSpell<FireBallConfigSource>
    {
        public GameObject ExplosionEffectPref;
        
        protected override void ImpactOnObject(GameObject hitObject)
        {
            base.ImpactOnObject(hitObject);

            ExplosionImpact(hitObject);
        }

        protected override void OnDestroyProjectile()
        {
            base.OnDestroyProjectile();

            Instantiate(ExplosionEffectPref, transform.position, transform.rotation);
        }
    }
}

using Rebel_Mage.Configs.Source;
using Unity.Mathematics;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class IceBall : BallSpell<IceBallConfigSource>
    {
        public GameObject ExplosionEffectPref;
        
        protected override void ImpactOnObject(GameObject hitObject)
        {
            base.ImpactOnObject(hitObject);

            if (hitObject.GetComponent<IImpact>() is {} impactSystem)
            {
                impactSystem.ChangeSpeedImpact(Config.SlowdownPercentage / 100, Config.TimeSlowdown);
            }
        }

        protected override void OnDestroyProjectile()
        {
            base.OnDestroyProjectile();

            Instantiate(ExplosionEffectPref, transform.position, quaternion.identity);
        }
    }
}

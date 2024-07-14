using PushItOut.Configs.Source;
using Unity.Mathematics;
using UnityEngine;

namespace PushItOut.Spell_system
{
    public class IceBall : BallProjectile
    {
        public IceBallConfigSource IceBallConfig;
        public GameObject ExplosionEffectPref;

        protected override void ImpactOnObject(GameObject hitObject)
        {
            base.ImpactOnObject(hitObject);

            if (hitObject.GetComponent<IImpact>() is {} impactSystem)
            {
                impactSystem.ChangeSpeedImpact(IceBallConfig.SlowdownPercentage / 100, IceBallConfig.TimeSlowdown);
            }
        }
        
        protected override void OnDestroyProjectile()
        {
            base.OnDestroyProjectile();
            
            Instantiate(ExplosionEffectPref, transform.position, quaternion.identity);
        }
    }
}

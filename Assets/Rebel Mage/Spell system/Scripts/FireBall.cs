﻿using PushItOut.Configs.Source;
using UnityEngine;

namespace PushItOut.Spell_system
{
    public class FireBall : BallProjectile
    {
        public FireBallConfigSource FireBallConfig;
        public GameObject ExplosionEffectPref;

        protected override void ImpactOnObject(GameObject hitObject)
        {
            base.ImpactOnObject(hitObject);

            if (hitObject.GetComponent<Rigidbody>() != null)
            {
                hitObject.GetComponent<Rigidbody>().AddExplosionForce(FireBallConfig.ExplosionForce, transform.position, FireBallConfig.ExplosionRadius);
            }

            if (hitObject.GetComponent<IImpact>() is {} impactSystem)
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
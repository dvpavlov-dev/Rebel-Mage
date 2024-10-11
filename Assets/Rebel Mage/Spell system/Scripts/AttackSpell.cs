using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public abstract class AttackSpell<T> : Spell<T> where T : AttackSpellConfig
    {
        protected virtual void HitHandling(GameObject other)
        {
            foreach (Collider hit in Physics.OverlapSphere(transform.position, Config.ExplosionRadius))
            {
                GameObject hitObject = hit.gameObject;

                ImpactOnObject(hitObject);
            }
        }
        
        protected virtual void ImpactOnObject(GameObject hitObject)
        {
            if (hitObject != Owner && hitObject.GetComponent<IDamage>() is {} damageSystemHit)
            {
                damageSystemHit.TakeDamage(Config.Damage);
            }
        }
        
        protected void ExplosionImpact(GameObject hitObject)
        {
            if (hitObject.GetComponent<Rigidbody>() != null)
            {
                hitObject.GetComponent<Rigidbody>().AddExplosionForce(Config.ExplosionForce, transform.position, Config.ExplosionRadius);
            }

            if (hitObject.GetComponent<IImpact>() is {} impactSystem) // impact on actors (player or enemy)
            {
                impactSystem.ExplosionImpact(transform.position, Config.ExplosionRadius, Config.ExplosionForce);
            }
        }
    }
}

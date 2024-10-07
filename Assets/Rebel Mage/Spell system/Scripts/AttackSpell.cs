using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public abstract class AttackSpell : Spell
    {
        private AttackSpellConfig AttackSpellConfig => Config as AttackSpellConfig;
        
        protected virtual void HitHandling(GameObject other)
        {
            foreach (Collider hit in Physics.OverlapSphere(transform.position, AttackSpellConfig.ExplosionRadius))
            {
                GameObject hitObject = hit.gameObject;

                ImpactOnObject(hitObject);
            }
        }
        
        protected virtual void ImpactOnObject(GameObject hitObject)
        {
            if (hitObject == _owner && hitObject.GetComponent<IDamage>() is {} damageSystemHit)
            {
                damageSystemHit.TakeDamage(AttackSpellConfig.Damage / 2);
            }
        }
    }
}

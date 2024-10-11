using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    [RequireComponent(typeof(Rigidbody))]
    public class MagicSurge : AttackSpell<MagicSurgeConfigSource>
    {
        public GameObject SpellEffect;
        
        private Rigidbody m_Rb;

        private void Awake()
        {
            m_Rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            HitHandling(Owner);

            Vector3 position = transform.position;
            
            m_Rb.AddExplosionForce(Config.ExplosionForce, position, Config.ExplosionRadius, 0, ForceMode.Acceleration);
            Instantiate(SpellEffect, position, Quaternion.identity);
            
            Destroy(gameObject);
        }

        protected override void ImpactOnObject(GameObject hitObject)
        {
            base.ImpactOnObject(hitObject);

            ExplosionImpact(hitObject);
        }

    }
}

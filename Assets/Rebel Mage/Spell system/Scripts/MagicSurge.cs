using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    [RequireComponent(typeof(Rigidbody))]
    public class MagicSurge : AttackSpell
    {
        public MagicSurgeConfigSource MagicSurgeConfig;
        public GameObject SpellEffect;
        
        protected override SpellConfig Config => MagicSurgeConfig;

        private Rigidbody m_Rb;

        private void Awake()
        {
            m_Rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            HitHandling(_owner);

            Vector3 position = transform.position;
            
            m_Rb.AddExplosionForce(MagicSurgeConfig.ExplosionForce, position, MagicSurgeConfig.ExplosionRadius, 0, ForceMode.Acceleration);
            Instantiate(SpellEffect, position, Quaternion.identity);
            
            Destroy(gameObject);
        }

        protected override void ImpactOnObject(GameObject hitObject)
        {
            base.ImpactOnObject(hitObject);
            
            if (hitObject.GetComponent<Rigidbody>() != null)
            {
                hitObject.GetComponent<Rigidbody>().AddExplosionForce(MagicSurgeConfig.ExplosionForce, transform.position, MagicSurgeConfig.ExplosionRadius);
            }
        }
    }
}

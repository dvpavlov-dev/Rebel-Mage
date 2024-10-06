using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class IceBallController : SpellController
    {
        public override void CastSpell()
        {
            m_Animator.SetTrigger(m_Config.AnimationName);
            Invoke(nameof(SpawnProjectile), 0.6f);
        }

        private void SpawnProjectile()
        {
            GameObject projectile = Instantiate(m_Config.SpellPrefab, m_SpellPoint.position, m_SpellPoint.rotation);
            projectile.GetComponent<Spell>().SetOwner(m_Owner);
        }
    }
}

using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class MagicSurgeController : SpellController
    {
        public override void CastSpell()
        {
            if (GetComponent<ICastSpells>() is {} actorCastSpell)
            {
                actorCastSpell.OnCastSpell(m_Config.AnimationTime);
            }
            
            m_Animator.SetTrigger(m_Config.AnimationName);
            Invoke(nameof(SpawnProjectile), 0.8f);
        }
        
        private void SpawnProjectile()
        {
            GameObject spell = Instantiate(m_Config.SpellPrefab, m_SpellPoint.position, Quaternion.identity);
            spell.GetComponent<Spell>().SetOwner(gameObject);
        }
    }
}

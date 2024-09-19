using Rebel_Mage.Infrastructure;
using Rebel_Mage.Spell_system.Configs;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Spell_system
{
    public class SpellsAction : MonoBehaviour
    {
        public Transform SpellPoint;
        public Animator AnimatorCastSpell;

        private Spells m_Spells;
        private IFactorySpells m_FactorySpells;

        [Inject]
        public void Constructor(Spells spells, IFactorySpells factorySpells)
        {
            m_Spells = spells;
            m_FactorySpells = factorySpells;
        }

        public void UseSpell(TypeSpell typeSpell)
        {
            if (m_Spells.TryCastSpell(typeSpell, out SpellConfig useSpell))
            {
                m_Spells.SetGlobalCooldown(typeSpell, useSpell);
                m_FactorySpells.CastSpell(gameObject, AnimatorCastSpell, SpellPoint, useSpell);
            }
        }
    }
}
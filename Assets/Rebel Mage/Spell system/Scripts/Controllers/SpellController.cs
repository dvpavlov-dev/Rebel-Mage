using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public abstract class SpellController : MonoBehaviour
    {
        protected Animator m_Animator;
        protected SpellConfig m_Config;
        protected GameObject m_Owner;
        protected Transform m_SpellPoint;

        public void Constructor(GameObject owner, Animator animator, Transform spellPoint, SpellConfig config)
        {
            m_Owner = owner;
            m_Animator = animator;
            m_Config = config;
            m_SpellPoint = spellPoint;
        }

        public abstract void CastSpell();
    }
}

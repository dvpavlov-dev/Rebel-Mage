using System.Collections.Generic;
using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class CooldownController
    {
        private readonly Dictionary<SpellConfig, float> m_CoolDown = new();
        private readonly Spells m_Spells;
        public CooldownController(Spells spells)
        {
            m_Spells = spells;
        }
        public void SetGlobalCooldown(TypeSpell typeSpell)
        {
            foreach (TypeSpell activeTypeSpell in m_Spells.ActiveSpells.Keys)
            {
                SpellConfig spell = m_Spells.ActiveSpells[activeTypeSpell];

                if (spell != null)
                {
                    m_CoolDown.TryAdd(spell, 0);

                    if (activeTypeSpell != typeSpell)
                    {
                        m_CoolDown[spell] = Time.time + spell.AnimationTime;
                        m_Spells.OnActivateCooldown?.Invoke(activeTypeSpell, Time.time, m_CoolDown[spell]);
                    }
                }
            }
        }
        public bool CheckCooldown(TypeSpell typeSpell, SpellConfig spell)
        {
            m_CoolDown.TryAdd(spell, 0);

            if (m_CoolDown[spell] <= Time.time)
            {
                m_CoolDown[spell] = Time.time + spell.Cooldown;
                m_Spells.OnActivateCooldown?.Invoke(typeSpell, Time.time, m_CoolDown[spell]);
                return true;
            }

            return false;
        }
    }
}

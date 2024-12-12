using System.Collections.Generic;
using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class CooldownController
    {
        private readonly Dictionary<SpellConfig, float> _coolDown = new();
        private readonly Spells _spells;
        
        public CooldownController(Spells spells)
        {
            _spells = spells;
        }
        
        public void SetGlobalCooldown(TypeSpell typeSpell)
        {
            foreach (TypeSpell activeTypeSpell in _spells.ActiveSpells.Keys)
            {
                SpellConfig spell = _spells.ActiveSpells[activeTypeSpell];

                if (spell != null)
                {
                    _coolDown.TryAdd(spell, 0);

                    if (activeTypeSpell != typeSpell)
                    {
                        _coolDown[spell] = Time.time + spell.AnimationTime;
                        _spells.OnActivateCooldown?.Invoke(activeTypeSpell, Time.time, _coolDown[spell]);
                    }
                }
            }
        }
        
        public bool CheckCooldown(TypeSpell typeSpell, SpellConfig spell)
        {
            _coolDown.TryAdd(spell, 0);

            if (_coolDown[spell] <= Time.time)
            {
                _coolDown[spell] = Time.time + spell.Cooldown;
                _spells.OnActivateCooldown?.Invoke(typeSpell, Time.time, _coolDown[spell]);
                return true;
            }

            return false;
        }
    }
}

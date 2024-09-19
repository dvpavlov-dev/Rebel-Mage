using System;
using System.Collections.Generic;
using System.Linq;
using Rebel_Mage.Spell_system.Configs;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class Spells : MonoBehaviour
    {
        public List<SpellConfig> AllSpells = new();
        public Action<TypeSpell, float, float> OnActivateCooldown;

        private readonly Dictionary<TypeSpell, SpellConfig> _activeSpells = new();
        private readonly Dictionary<SpellConfig, float> _coolDown = new();

        public void ClearSpells()
        {
            _activeSpells.Clear();

            SetSpell(AllSpells[0], TypeSpell.BASE_ATTACK);
        }

        public Dictionary<TypeSpell, SpellConfig> GetActiveSpells()
        {
            return _activeSpells;
        }

        public void SetSpell(SpellConfig spell, TypeSpell typeSpell)
        {
            _activeSpells[typeSpell] = spell;
        }

        public bool TryGetSpell(TypeSpell typeSpell, out SpellConfig spell)
        {
            if (_activeSpells.ContainsKey(typeSpell) && _activeSpells[typeSpell] != null)
            {
                spell = _activeSpells[typeSpell];
                return true;
            }

            spell = null;
            return false;
        }

        public bool TryCastSpell(TypeSpell typeSpell, out SpellConfig spellConfig)
        {
            if (TryGetSpell(typeSpell, out SpellConfig useSpell))
            {
                if (CheckCooldown(typeSpell, useSpell))
                {
                    spellConfig = useSpell;
                    return true;
                }
            }

            spellConfig = null;
            return false;
        }

        public void SetGlobalCooldown(TypeSpell typeSpell, SpellConfig spellConfig)
        {
            foreach (TypeSpell activeTypeSpell in _activeSpells.Keys)
            {
                SpellConfig spell = _activeSpells[activeTypeSpell];
                
                if (spell != null)
                {
                    _coolDown.TryAdd(spell, 0);

                    if (activeTypeSpell != typeSpell)
                    {
                        _coolDown[spell] = Time.time + spell.AnimationTime;
                        OnActivateCooldown?.Invoke(activeTypeSpell, Time.time, _coolDown[spell]);
                    }
                }
            }
        }

        private bool CheckCooldown(TypeSpell typeSpell, SpellConfig spell)
        {
            _coolDown.TryAdd(spell, 0);

            if (_coolDown[spell] <= Time.time)
            {
                _coolDown[spell] = Time.time + spell.Cooldown;
                OnActivateCooldown?.Invoke(typeSpell, Time.time, _coolDown[spell]);
                return true;
            }

            return false;
        }
    }
}

public enum TypeSpell
{
    BASE_ATTACK,
    SUPPORT_ATTACK,
    FIRST_SPELL,
    SECOND_SPELL,
    THIRD_SPELL,
    SHIFT_SPELL,
}

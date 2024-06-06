using System;
using System.Collections.Generic;
using PushItOut.Spell_system.Configs;
using UnityEngine;

namespace PushItOut.Spell_system
{
    public class Spells : MonoBehaviour
    {
        public List<SpellConfig> AllSpells = new();
        public Action<TypeSpell, float, float> OnActivateCooldown;

        private readonly Dictionary<TypeSpell, SpellConfig> _activeSpells = new();
        private readonly Dictionary<SpellConfig, float> _coolDown = new();

        // void Start()
        // {
        //     TestSetSpells();
        // }
        // void TestSetSpells()
        // {
        //     SetSpell(AllSpells[0], TypeSpell.SHIFT_SPELL);
        //     SetSpell(AllSpells[1], TypeSpell.BASE_ATTACK);
        //     SetSpell(AllSpells[2], TypeSpell.SUPPORT_ATTACK);
        // }

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

        public bool CheckCooldown(TypeSpell typeSpell, SpellConfig spell)
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

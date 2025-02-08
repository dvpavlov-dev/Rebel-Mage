using System;
using System.Collections.Generic;
using Rebel_Mage.Configs.Source;
using Rebel_Mage.Infrastructure;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Spell_system
{

    public class Spells : MonoBehaviour
    {
        public List<SpellConfig> AllSpells => _allSpells;

        public Action<TypeSpell, float, float> OnActivateCooldown { get; set; }

        public Dictionary<TypeSpell, SpellConfig> ActiveSpells { get; } = new();

        [SerializeField] private List<SpellConfig> _allSpells;
        
        private CooldownController CooldownController => _cooldownController ??= new CooldownController(this);
        private CooldownController _cooldownController;
        private ISpellsFactory _spellsFactory;

        [Inject]
        public void Constructor(ISpellsFactory spellsFactory)
        {
            _spellsFactory = spellsFactory;
        }
        
        public void ClearSpells()
        {
            ActiveSpells.Clear();

            SetActiveSpell(AllSpells[0], TypeSpell.BASE_ATTACK);
        }

        public void SetActiveSpell(SpellConfig spell, TypeSpell typeSpell)
        {
            ActiveSpells[typeSpell] = spell;
        }

        public bool TryGetSpell(TypeSpell typeSpell, out SpellConfig spell)
        {
            if (ActiveSpells.ContainsKey(typeSpell) && ActiveSpells[typeSpell] != null)
            {
                spell = ActiveSpells[typeSpell];
                return true;
            }

            spell = null;
            return false;
        }

        public void CastSpell(TypeSpell typeSpell, Animator animatorCastSpell, Transform spellPoint, GameObject owner)
        {
            if (TryGetSpell(typeSpell, out SpellConfig useSpell) && CooldownController.CheckCooldown(typeSpell, useSpell))
            {
                CooldownController.SetGlobalCooldown(typeSpell);
                _spellsFactory.CastSpell(owner, animatorCastSpell, spellPoint, useSpell);
            }
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

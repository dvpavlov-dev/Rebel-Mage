using System.Collections.Generic;
using Rebel_Mage.Configs.Source;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Spell_system
{
    public class SpellPanelController : MonoBehaviour
    {
        public SpellCell BaseSpell;
        public SpellCell SupportSpell;

        public List<SpellCell> OtherSpells = new();

        public SpellCell ShiftSpell;

        private Dictionary<TypeSpell, SpellCell> _spellsInPanel = new();

        private Spells _spells;

        [Inject]
        public void Constructor(Spells spells)
        {
            _spells = spells;
            _spells.OnActivateCooldown += ActivateCooldown;

            UpdatePanel();
        }

        private void ActivateCooldown(TypeSpell typeSpell, float startTime, float endTime)
        {
            _spellsInPanel[typeSpell].CooldownAnimation(startTime, endTime);
        }

        private void UpdateSpellCell(TypeSpell typeSpell, SpellCell spellCell)
        {
            if (_spells.TryGetSpell(typeSpell, out SpellConfig spell))
            {
                spellCell.SpellImage.sprite = spell.SpellImage;
                if (!_spellsInPanel.TryAdd(typeSpell, spellCell))
                {
                    _spellsInPanel[typeSpell] = spellCell;
                }
            }
        }

        private void UpdatePanel()
        {
            UpdateSpellCell(TypeSpell.BASE_ATTACK, BaseSpell);
            UpdateSpellCell(TypeSpell.SUPPORT_ATTACK, SupportSpell);
            UpdateSpellCell(TypeSpell.FIRST_SPELL, OtherSpells[0]);
            UpdateSpellCell(TypeSpell.SECOND_SPELL, OtherSpells[1]);
            UpdateSpellCell(TypeSpell.THIRD_SPELL, OtherSpells[2]);
            UpdateSpellCell(TypeSpell.SHIFT_SPELL, ShiftSpell);
        }

        void OnDestroy()
        {
            _spells.OnActivateCooldown -= ActivateCooldown;
        }

        private void OnEnable()
        {
            UpdatePanel();
        }
    }
}
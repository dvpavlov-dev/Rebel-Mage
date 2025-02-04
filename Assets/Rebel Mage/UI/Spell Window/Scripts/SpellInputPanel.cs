using System.Collections.Generic;
using Rebel_Mage.Configs.Source;
using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.UI
{
    public class SpellInputPanel : MonoBehaviour
    {
        public List<SlotSpellInput> Slots = new();

        private SpellWindowController _spellWindowController;
        private SlotSpellInput _currentSelectedSlot;
        private Spells _spells;
        
        public void InitInputPanel(SpellWindowController spellWindowController, Spells spells)
        {
            _spellWindowController = spellWindowController;
            
            Slots[0].SpellSlotInit("Left mouse", TypeSpell.BASE_ATTACK);
            Slots[1].SpellSlotInit("Right mouse", TypeSpell.SUPPORT_ATTACK);
            Slots[2].SpellSlotInit("Q", TypeSpell.FIRST_SPELL);
            Slots[3].SpellSlotInit("E", TypeSpell.SECOND_SPELL);
            Slots[4].SpellSlotInit("R", TypeSpell.THIRD_SPELL);
            Slots[5].SpellSlotInit("Shift", TypeSpell.SHIFT_SPELL);

            foreach (SlotSpellInput slot in Slots)
            {
                slot.InitSlot(this);
                slot.SetEmptySlot();
            }

            foreach (KeyValuePair<TypeSpell, SpellConfig> activeSpell in spells.ActiveSpells)
            {
                foreach (SlotSpellInput slot in Slots)
                {
                    if (activeSpell.Key == slot.TypeSpellSlot)
                    {
                        if (activeSpell.Value != null)
                        {
                            slot.SetSlot(activeSpell.Value);
                        }
                        
                        break;
                    }
                }
            }

            _spells = spells;
        }

        public void SaveSpellsInSlots()
        {
            foreach (SlotSpellInput slot in Slots)
            {
                _spells.SetActiveSpell(slot.GetSpell(), slot.TypeSpellSlot);
            }
        }

        public void SyncWindowState(SpellWindowState state)
        {
            switch (state)
            {
                case SpellWindowState.IDLE:
                    foreach (SlotSpellInput inputSlot in Slots)
                    {
                        inputSlot.UnselectedSlot();
                    }

                    if (_spellWindowController.ChoosingSpell != null)
                    {
                        if (_spellWindowController.ChoosingSpell != _currentSelectedSlot.GetSpell())
                        {
                            foreach (SlotSpellInput spellInput in Slots)
                            {
                                if (spellInput.GetSpell() == _spellWindowController.ChoosingSpell)
                                {
                                    spellInput.SetEmptySlot();
                                }
                            }
                            
                            _currentSelectedSlot.SetSlot(_spellWindowController.ChoosingSpell);
                        }
                        
                        _currentSelectedSlot = null;
                    }
                    
                    break;

                case SpellWindowState.CHOICE_INPUT_PANEL:
                case SpellWindowState.CHOICE_COLLECTION:
                    foreach (SlotSpellInput inputSlot in Slots)
                    {
                        if (_currentSelectedSlot != null && inputSlot == _currentSelectedSlot)
                        {
                            continue;
                        }

                        inputSlot.WaitingSlot();
                    }
                    
                    break;
            }
        }

        public void RemoveSpellFromInput(SlotSpellInput slot)
        {
            int spellCount = 0;
            
            foreach (SlotSpellInput spellInput in Slots)
            {
                if (spellInput.GetSpell() != null)
                {
                    spellCount++;
                }
            }

            if (spellCount <= 1)
            {
                return;
            }
            
            slot.SetEmptySlot();
            _currentSelectedSlot = null;
            _spellWindowController.ChoosingSpell = null;
            _spellWindowController.SyncWindowState(SpellWindowState.IDLE);
        }

        public void SlotAction(SlotSpellInput slot)
        {
            switch (_spellWindowController.WindowState)
            {
                case SpellWindowState.IDLE:
                    _currentSelectedSlot = slot;
                    _spellWindowController.ChoosingSpell = slot.GetSpell();
                    slot.SelectedSlot();
                    _spellWindowController.SyncWindowState(SpellWindowState.CHOICE_INPUT_PANEL);
                    break;

                case SpellWindowState.CHOICE_INPUT_PANEL:
                    if (_currentSelectedSlot != null && _currentSelectedSlot != slot)
                    {
                        if (_currentSelectedSlot.GetSpell() != null)
                        {
                            if (slot.GetSpell() != null)
                            {
                                SpellConfig changeSlot = _currentSelectedSlot.GetSpell();
                                _currentSelectedSlot.SetSlot(slot.GetSpell());
                                slot.SetSlot(changeSlot);
                                // _currentSelectedSlot = null;
                            }
                            else
                            {
                                slot.SetSlot(_currentSelectedSlot.GetSpell());
                                _currentSelectedSlot.SetEmptySlot();
                            }
                        }
                        else
                        {
                            if (slot.GetSpell() != null)
                            {
                                _currentSelectedSlot.SetSlot(slot.GetSpell());
                                slot.SetEmptySlot();
                            }
                        }
                    }
                    else
                    {
                        slot.UnselectedSlot();
                    }
                    
                    _currentSelectedSlot = null;
                    _spellWindowController.ChoosingSpell = null;
                    _spellWindowController.SyncWindowState(SpellWindowState.IDLE);
                    break;
                
                case SpellWindowState.CHOICE_COLLECTION:
                    if (_spellWindowController.ChoosingSpell == slot.GetSpell())
                    {
                        _spellWindowController.ChoosingSpell = null;
                        slot.UnselectedSlot();
                        _currentSelectedSlot = null;
                        _spellWindowController.SyncWindowState(SpellWindowState.IDLE);
                    }
                    else
                    {
                        foreach (SlotSpellInput spellInput in Slots)
                        {
                            if (spellInput.GetSpell() == _spellWindowController.ChoosingSpell)
                            {
                                spellInput.SetEmptySlot();
                            }
                        }
                        
                        slot.SetSlot(_spellWindowController.ChoosingSpell);
                        _spellWindowController.ChoosingSpell = null;
                        slot.UnselectedSlot();
                        _currentSelectedSlot = null;
                        _spellWindowController.SyncWindowState(SpellWindowState.IDLE);
                    }
                    
                    break;
            }
        }
    }
}

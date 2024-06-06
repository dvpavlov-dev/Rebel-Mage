using System.Collections.Generic;
using ModestTree;
using PushItOut.Spell_system;
using PushItOut.Spell_system.Configs;
using UnityEngine;
using Zenject;

namespace PushItOut.UI.Spell_Window
{
    public class SpellCollection : MonoBehaviour
    {
        public GameObject SpellCollectionCellPrefab;

        private List<SpellCollectionСell> _spellSetСells = new();
        private SpellWindowController _spellWindowController;
        private SpellCollectionСell _currentSelectedSpellCell;

        [Inject]
        public void Constructor(SpellWindowController spellWindowController)
        {
            _spellWindowController = spellWindowController;
        }

        private void ClearSpellCollection()
        {
            if (_spellSetСells.IsEmpty())
            {
                return;
            }
            
            foreach (SpellCollectionСell spellCell in _spellSetСells)
            {
                Destroy(spellCell.gameObject);
            }
            
            _spellSetСells.Clear();
        }

        public void InitSpellCollection(Spells spells)
        {
            ClearSpellCollection();
            
            foreach (SpellConfig spell in spells.AllSpells)
            {
                GameObject spellSetCellObj = Instantiate(SpellCollectionCellPrefab, transform);
                SpellCollectionСell spellCollectionCell = spellSetCellObj.GetComponent<SpellCollectionСell>();
                spellCollectionCell.InitSpellSetCell(spell, this);
                _spellSetСells.Add(spellCollectionCell);
            }
        }

        public void SyncWindowState(SpellWindowState state)
        {
            if (state == SpellWindowState.IDLE)
            {
                foreach (SpellCollectionСell collectionCell in _spellSetСells)
                {
                    collectionCell.UnselectedCell();
                }
            }
        }

        public void CellAction(SpellCollectionСell cell)
        {
            switch (_spellWindowController.WindowState)
            {
                case SpellWindowState.IDLE:
                    _currentSelectedSpellCell = cell;
                    _spellWindowController.ChoosingSpell = cell.GetSpell();
                    cell.SelectedCell();
                    _spellWindowController.SyncWindowState(SpellWindowState.CHOICE_COLLECTION);
                    break;
                
                case SpellWindowState.CHOICE_INPUT_PANEL:
                    _spellWindowController.ChoosingSpell = cell.GetSpell();
                    cell.UnselectedCell();
                    _currentSelectedSpellCell = null;
                    _spellWindowController.SyncWindowState(SpellWindowState.IDLE);
                    break;

                case SpellWindowState.CHOICE_COLLECTION:
                    if (_currentSelectedSpellCell != null && _currentSelectedSpellCell != cell)
                    {
                        _currentSelectedSpellCell.UnselectedCell();
                        cell.SelectedCell();
                        _currentSelectedSpellCell = cell;
                    }
                    else
                    {
                        cell.UnselectedCell();
                        _currentSelectedSpellCell = null;
                        _spellWindowController.ChoosingSpell = null;
                        _spellWindowController.SyncWindowState(SpellWindowState.IDLE);
                    }

                    break;
            }
        }
    }
}

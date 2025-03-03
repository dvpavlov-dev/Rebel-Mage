using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using System.Linq;
using Rebel_Mage.Configs.Source;
using Rebel_Mage.Infrastructure;
using Rebel_Mage.Spell_system;

namespace Rebel_Mage.UI
{
    public class SpellCollection : MonoBehaviour
    {
        public GameObject SpellCollectionCellPrefab;

        private readonly List<SpellCollectionСell> _spellCollectionCells = new();
        
        private SpellWindowController _spellWindowController;
        private SpellCollectionСell _currentSelectedSpellCell;

        private void ClearSpellCollection()
        {
            if (_spellCollectionCells.IsEmpty())
            {
                return;
            }
            
            foreach (SpellCollectionСell spellCell in _spellCollectionCells)
            {
                Destroy(spellCell.gameObject);
            }
            
            _spellCollectionCells.Clear();
        }

        private List<SpellConfig> SortByOpenRound(List<SpellConfig> spells)
        {
            return spells.OrderBy(x => x.OpenAfterRound).ToList();
        }

        public void InitSpellCollection(SpellWindowController spellWindowController, Spells spells, IRoundProcess roundProcess)
        {
            _spellWindowController = spellWindowController;
            
            ClearSpellCollection();
            
            foreach (SpellConfig spell in SortByOpenRound(spells.AllSpells))
            {
                GameObject spellSetCellObj = Instantiate(SpellCollectionCellPrefab, transform);
                SpellCollectionСell spellCollectionCell = spellSetCellObj.GetComponent<SpellCollectionСell>();
                spellCollectionCell.InitSpellSetCell(spell, this, roundProcess);
                _spellCollectionCells.Add(spellCollectionCell);
            }
        }

        public void SyncWindowState(SpellWindowState state)
        {
            if (state == SpellWindowState.IDLE)
            {
                foreach (SpellCollectionСell collectionCell in _spellCollectionCells)
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
                        _spellWindowController.ChoosingSpell = cell.GetSpell();
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

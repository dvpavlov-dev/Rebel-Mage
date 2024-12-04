using System;
using Rebel_Mage.Configs.Source;
using Rebel_Mage.Infrastructure;
using Rebel_Mage.Spell_system;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Rebel_Mage.UI.Spell_Window
{
    public class SpellWindowController : MonoBehaviour
    {
        public SpellCollection SpellCollection;
        public SpellInputPanel InputPanel;

        [SerializeField] private Button readyButton;
        
        private IRoundProcess _roundProcess;
        private Spells _spells;

        public Action OnFinishedChooseSpells { get; set; }
        public SpellWindowState WindowState { get; private set; }
        public SpellConfig ChoosingSpell { get; set; }

        [Inject]
        public void Constructor(Spells spells, IRoundProcess roundProcess)
        {
            _spells = spells;
            _roundProcess = roundProcess;
        }

        public void InitSpellWindow()
        {
            readyButton.interactable = true;
            
            SpellCollection.InitSpellCollection(this, _spells, _roundProcess);
            InputPanel.InitInputPanel(this, _spells);
        }

        public void SyncWindowState(SpellWindowState state)
        {
            WindowState = state;

            SpellCollection.SyncWindowState(WindowState);
            InputPanel.SyncWindowState(WindowState);
        }

        public void OnClickReadyButton()
        {
            readyButton.interactable = false;
            
            InputPanel.SaveSpellsInSlots();
            Invoke(nameof(StartGame), 1f);
        }

        private void StartGame()
        {
            OnFinishedChooseSpells?.Invoke();
        }
    }

    public enum SpellWindowState
    {
        IDLE,
        CHOICE_INPUT_PANEL,
        CHOICE_COLLECTION,
    }
}

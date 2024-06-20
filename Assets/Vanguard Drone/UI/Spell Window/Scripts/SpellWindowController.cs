using System;
using PushItOut.Spell_system;
using PushItOut.Spell_system.Configs;
using UnityEngine;
using Vanguard_Drone.Infrastructure;
using Zenject;

namespace PushItOut.UI.Spell_Window
{
    public class SpellWindowController : MonoBehaviour
    {
        public SpellCollection SpellCollection;
        public SpellInputPanel InputPanel;

        public Action OnFinishedChooseSpells;
        
        public SpellWindowState WindowState { get; private set; }
        public SpellConfig ChoosingSpell { get; set; }

        private Spells _spells;
        private RoundProcess _roundProcess;

        [Inject]
        public void Constructor(Spells spells, RoundProcess roundProcess)
        {
            _spells = spells;
            _roundProcess = roundProcess;
        }

        public void InitSpellWindow()
        {
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
            InputPanel.SaveSpellsInSlots();
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

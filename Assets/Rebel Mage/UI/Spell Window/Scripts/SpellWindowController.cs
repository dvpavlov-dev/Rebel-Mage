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

        private RoundProcess _roundProcess;
        private Spells _spells;

        public Action OnFinishedChooseSpells { get; set; }
        public SpellWindowState WindowState { get; private set; }
        public SpellConfig ChoosingSpell { get; set; }

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
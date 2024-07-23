using PushItOut.UI.Gameplay;
using PushItOut.UI.Spell_Window;
using UnityEngine;
using Zenject;

namespace Vanguard_Drone.Infrastructure
{
    public class Game : MonoBehaviour
    {
        private GameStateMachine _stateMachine;

        private IRoundProcess _roundProcess;
        private SpellWindowController _spellWindowController;
        private GameplayUI _gameplayUI;

        [Inject]
        private void Constructor(IRoundProcess roundProcess, SpellWindowController spellWindowController, GameplayUI gameplayUI)
        {
            _roundProcess = roundProcess;
            _spellWindowController = spellWindowController;
            _gameplayUI = gameplayUI;
        }

        void Start()
        {
            _stateMachine = new GameStateMachine(_roundProcess, _spellWindowController, _gameplayUI);
        }
    }
}

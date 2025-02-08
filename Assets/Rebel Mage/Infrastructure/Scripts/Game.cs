using Rebel_Mage.UI;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Infrastructure
{
    public class Game : MonoBehaviour
    {
        private GameStateMachine _stateMachine;
        private IRoundProcess _roundProcess;
        private SpellWindowController _spellWindowController;
        private GameplayUI _gameplayUI;
        private IActorsFactory _actorsFactory;
        private ILoadingSceneService _loadingSceneService;

        [Inject]
        private void Constructor(IRoundProcess roundProcess, SpellWindowController spellWindowController, GameplayUI gameplayUI, IActorsFactory actorsFactory, ILoadingSceneService loadingSceneService)
        {
            _loadingSceneService = loadingSceneService;
            _actorsFactory = actorsFactory;
            _roundProcess = roundProcess;
            _spellWindowController = spellWindowController;
            _gameplayUI = gameplayUI;

            _loadingSceneService.OnSceneLoaded = StartGame;
        }

        private void StartGame()
        {
            _stateMachine = new GameStateMachine(_roundProcess, _spellWindowController, _gameplayUI, _actorsFactory);
        }
    }
}

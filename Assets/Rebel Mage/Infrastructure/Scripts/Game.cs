using Rebel_Mage.Enemy;
using Rebel_Mage.UI.Gameplay;
using Rebel_Mage.UI.Spell_Window;
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
        private IEnemySpawner _enemySpawner;
        private IFactoryActors _factoryActors;
        private ILoadingSceneService _loadingSceneService;

        [Inject]
        private void Constructor(IRoundProcess roundProcess, SpellWindowController spellWindowController, GameplayUI gameplayUI, IEnemySpawner enemySpawner, IFactoryActors factoryActors, ILoadingSceneService loadingSceneService)
        {
            _loadingSceneService = loadingSceneService;
            _factoryActors = factoryActors;
            _enemySpawner = enemySpawner;
            _roundProcess = roundProcess;
            _spellWindowController = spellWindowController;
            _gameplayUI = gameplayUI;

            _loadingSceneService.OnSceneLoaded = StartGame;
        }

        private void StartGame()
        {
            _stateMachine = new GameStateMachine(_roundProcess, _spellWindowController, _gameplayUI, _enemySpawner, _factoryActors);
        }
    }
}

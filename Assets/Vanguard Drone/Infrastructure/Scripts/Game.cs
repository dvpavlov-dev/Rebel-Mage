using PushItOut.UI.Gameplay;
using PushItOut.UI.Spell_Window;
using UnityEngine;
using Vanguard_Drone.Enemy;
using Vanguard_Drone.Infrastructure;
using Zenject;

public class Game : MonoBehaviour
{
    private GameStateMachine _stateMachine;

    private RoundProcess _roundProcess;
    private Factory _factory;
    private SpellWindowController _spellWindowController;
    private GameplayUI _gameplayUI;
    private EnemySpawner _enemySpawner;

    [Inject]
    private void Constructor(RoundProcess roundProcess, Factory factory, SpellWindowController spellWindowController, GameplayUI gameplayUI, EnemySpawner enemySpawner)
    {
        _roundProcess = roundProcess;
        _factory = factory;
        _spellWindowController = spellWindowController;
        _gameplayUI = gameplayUI;
        _enemySpawner = enemySpawner;
    }
    
    void Start()
    {
        _stateMachine = new GameStateMachine(_roundProcess, _factory, _spellWindowController, _gameplayUI, _enemySpawner);
    }
}

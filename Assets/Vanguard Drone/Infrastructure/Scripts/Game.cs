using PushItOut.UI.Gameplay;
using PushItOut.UI.Spell_Window;
using UnityEngine;
using Vanguard_Drone.Infrastructure;
using Zenject;

public class Game : MonoBehaviour
{
    private GameStateMachine _stateMachine;

    private RoundProcess _roundProcess;
    private SpellWindowController _spellWindowController;
    private GameplayUI _gameplayUI;

    [Inject]
    private void Constructor(RoundProcess roundProcess, SpellWindowController spellWindowController, GameplayUI gameplayUI)
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

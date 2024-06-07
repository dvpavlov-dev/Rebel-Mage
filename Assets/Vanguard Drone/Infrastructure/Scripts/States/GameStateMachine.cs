using System.Collections.Generic;
using PushItOut.UI.Gameplay;
using PushItOut.UI.Spell_Window;
using UnityEngine;
using Vanguard_Drone.Enemy;

namespace Vanguard_Drone.Infrastructure 
{
    public class GameStateMachine
    {
        private readonly Dictionary<TypeState, IState> States;
        private IState activeState;

        public GameStateMachine(RoundProcess roundProcess, Factory factory, SpellWindowController spellWindowController, GameplayUI gameplayUI, EnemySpawner enemySpawner)
        {
            States = new Dictionary<TypeState, IState>
            {
                [TypeState.START_GAME] = new StartGame(this),
                [TypeState.CHANGE_ABILITY] = new ChangeAbility(this, spellWindowController),
                [TypeState.START_ROUND] = new StartRound(this, factory, roundProcess, gameplayUI, enemySpawner),
                [TypeState.END_ROUND] = new EndRound(this),
                [TypeState.PLAYER_LOOSE] = new PlayerLoose(this),
            };
            
            ChangeState(TypeState.START_GAME);
        }

        public void ChangeState(TypeState typeState)
        {
            activeState?.Exit();
            activeState = States[typeState];
            activeState.Enter();
        }
    }

    public interface IState
    {
        public void Enter();
        public void Exit();
    }

    public enum TypeState
    {
        START_GAME,
        CHANGE_ABILITY,
        START_ROUND,
        END_ROUND,
        PLAYER_LOOSE,
    }

    public class StartGame : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        
        public StartGame(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter()
        {
            _gameStateMachine.ChangeState(TypeState.CHANGE_ABILITY);
        }
        
        public void Exit()
        {
        }
    }

    public class ChangeAbility : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SpellWindowController _spellWindowController;
        
        public ChangeAbility(GameStateMachine gameStateMachine, SpellWindowController spellWindowController)
        {
            _gameStateMachine = gameStateMachine;
            _spellWindowController = spellWindowController;
        }
        
        public void Enter()
        {
            _spellWindowController.gameObject.SetActive(true);
            _spellWindowController.InitSpellWindow();
            _spellWindowController.OnFinishedChooseSpells += ChooseSpellsFinished;
        }
        
        public void Exit()
        {
            _spellWindowController.gameObject.SetActive(false);
        }

        private void ChooseSpellsFinished()
        {
            _spellWindowController.OnFinishedChooseSpells -= ChooseSpellsFinished;
            _gameStateMachine.ChangeState(TypeState.START_ROUND);
        }
    }

    public class StartRound : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly Factory _factory;
        private readonly RoundProcess _roundProcess;
        private readonly GameplayUI _gameplayUI;
        private readonly EnemySpawner _enemySpawner;

        public StartRound(GameStateMachine gameStateMachine, Factory factory, RoundProcess roundProcess, GameplayUI gameplayUI, EnemySpawner enemySpawner)
        {
            _gameStateMachine = gameStateMachine;
            _factory = factory;
            _roundProcess = roundProcess;
            _gameplayUI = gameplayUI;
            _enemySpawner = enemySpawner;
        }
        
        public void Enter()
        {
            _gameplayUI.SpellsPanel.SetActive(true);

            GameObject player = _factory.CreatePlayer(new Vector3(0, 1, 0));
            _enemySpawner.InitEnemySpawner(_factory, player);
            // _factory.CreateEnemy(EnemyType.BASE_ENEMY ,new Vector3(20, 1, 0), player);
            
            _roundProcess.StartRound();
        }
        
        public void Exit()
        {
            _gameplayUI.SpellsPanel.SetActive(false);
        }
    }

    public class EndRound : IState
    {
        public EndRound(GameStateMachine gameStateMachine)
        {
        }
        
        public void Enter()
        {
            throw new System.NotImplementedException();
        }
        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class PlayerLoose : IState
    {
        public PlayerLoose(GameStateMachine gameStateMachine)
        {
        }
        
        public void Enter()
        {
            throw new System.NotImplementedException();
        }
        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}
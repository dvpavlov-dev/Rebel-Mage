using System;
using System.Collections.Generic;
using Rebel_Mage.UI;

namespace Rebel_Mage.Infrastructure 
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        
        private IExitableState _activeState;

        public GameStateMachine(IRoundProcess roundProcess, SpellWindowController spellWindowController, GameplayUI gameplayUI, IActorsFactory actorsFactory)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(StartGame)] = new StartGame(this, actorsFactory),
                [typeof(ChangeAbility)] = new ChangeAbility(this, spellWindowController),
                [typeof(StartRound)] = new StartRound(this, roundProcess, gameplayUI),
                [typeof(EndRound)] = new EndRound(this, gameplayUI, roundProcess),
                [typeof(EndGame)] = new EndGame(this, gameplayUI, roundProcess),
                [typeof(PlayerLoose)] = new PlayerLoose(this, gameplayUI, roundProcess),
            };

            Enter<StartGame>();
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }
        
        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
                
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }

    public interface IState : IExitableState
    {
        public void Enter();
    }

    public interface IExitableState
    {
        public void Exit();
    }
    
    public class StartGame : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IActorsFactory _actorsFactory;

        public StartGame(GameStateMachine gameStateMachine, IActorsFactory actorsFactory)
        {
            _gameStateMachine = gameStateMachine;
            _actorsFactory = actorsFactory;
        }
        
        public void Enter()
        {
            _actorsFactory.InitFactoryActors(InitFactoryActorsEnded);
        }
        
        public void Exit()
        {
        }

        private void InitFactoryActorsEnded()
        {
            _gameStateMachine.Enter<ChangeAbility>();
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
            
            BackgroundMusicController.Instance.ActivateChooseSpellClip();
        }
        
        public void Exit()
        {
            _spellWindowController.OnFinishedChooseSpells -= ChooseSpellsFinished;
            _spellWindowController.gameObject.SetActive(false);
        }

        private void ChooseSpellsFinished()
        {
            _gameStateMachine.Enter<StartRound>();
        }
    }

    public class StartRound : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IRoundProcess _roundProcess;
        private readonly GameplayUI _gameplayUI;

        public StartRound(GameStateMachine gameStateMachine, IRoundProcess roundProcess, GameplayUI gameplayUI)
        {
            _gameStateMachine = gameStateMachine;
            _roundProcess = roundProcess;
            _gameplayUI = gameplayUI;
        }
        
        public void Enter()
        {
            _gameplayUI.SpellsPanel.SetActive(true);
            _roundProcess.StartRound();
            SubscribeToEvents();
            
            BackgroundMusicController.Instance.ActivateBattleClip();
        }
        
        public void Exit()
        {
            UnsubscribeFromEvents();
            _gameplayUI.SpellsPanel.SetActive(false);
        }
        
        private void SubscribeToEvents()
        {
            _roundProcess.OnEndRound += FinishTheRound;
            _roundProcess.OnEndGame += EndingGame;
            _roundProcess.OnPlayerLost += PlayerLost;
        }
        
        private void UnsubscribeFromEvents()
        {
            _roundProcess.OnEndRound -= FinishTheRound;
            _roundProcess.OnEndGame -= EndingGame;
            _roundProcess.OnPlayerLost -= PlayerLost;
        }

        private void PlayerLost()
        {
            _gameStateMachine.Enter<PlayerLoose>();
        }

        private void EndingGame()
        {
            _gameStateMachine.Enter<EndGame>();
        }

        private void FinishTheRound()
        {
            _gameStateMachine.Enter<EndRound>();
        }
    }

    public class EndRound : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly GameplayUI _gameplayUI;
        private readonly IRoundProcess _roundProcess;

        public EndRound(GameStateMachine gameStateMachine, GameplayUI gameplayUI, IRoundProcess roundProcess)
        {
            _gameStateMachine = gameStateMachine;
            _gameplayUI = gameplayUI;
            _roundProcess = roundProcess;
        }
        
        public void Enter()
        {
            _gameplayUI.RoundOver(true, _roundProcess.PointsForAllRounds);
            _gameplayUI.OnContinuePlay = () => _gameStateMachine.Enter<ChangeAbility>();
        }
        
        public void Exit()
        {
        }
    }
    
    public class EndGame : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly GameplayUI _gameplayUI;
        private readonly IRoundProcess _roundProcess;

        public EndGame(GameStateMachine gameStateMachine, GameplayUI gameplayUI, IRoundProcess roundProcess)
        {
            _gameStateMachine = gameStateMachine;
            _gameplayUI = gameplayUI;
            _roundProcess = roundProcess;
        }
        
        public void Enter()
        {
            _gameplayUI.RoundOver(true, _roundProcess.PointsForAllRounds);
            _gameplayUI.OnContinuePlay = () => _gameStateMachine.Enter<ChangeAbility>();
        }
        
        public void Exit()
        {
        }
    }

    public class PlayerLoose : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly GameplayUI _gameplayUI;
        private readonly IRoundProcess _roundProcess;

        public PlayerLoose(GameStateMachine gameStateMachine, GameplayUI gameplayUI, IRoundProcess roundProcess)
        {
            _gameStateMachine = gameStateMachine;
            _gameplayUI = gameplayUI;
            _roundProcess = roundProcess;
        }
        
        public void Enter()
        {
            _gameplayUI.RoundOver(false, _roundProcess.PointsForAllRounds);
            _gameplayUI.OnContinuePlay = () => _gameStateMachine.Enter<ChangeAbility>();
        }
        
        public void Exit()
        {
        }
    }
}
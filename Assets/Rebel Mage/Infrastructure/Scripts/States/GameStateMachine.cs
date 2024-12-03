using System.Collections.Generic;
using Rebel_Mage.UI.Gameplay;
using Rebel_Mage.UI.Spell_Window;

namespace Rebel_Mage.Infrastructure 
{
    public class GameStateMachine
    {
        private readonly Dictionary<TypeState, IState> States;
        private IState activeState;

        public GameStateMachine(IRoundProcess roundProcess, SpellWindowController spellWindowController, GameplayUI gameplayUI)
        {
            States = new Dictionary<TypeState, IState>
            {
                [TypeState.START_GAME] = new StartGame(this),
                [TypeState.CHANGE_ABILITY] = new ChangeAbility(this, spellWindowController),
                [TypeState.START_ROUND] = new StartRound(this, roundProcess, gameplayUI),
                [TypeState.END_ROUND] = new EndRound(this, gameplayUI, roundProcess),
                [TypeState.END_GAME] = new EndGame(this, gameplayUI, roundProcess),
                [TypeState.PLAYER_LOST] = new PlayerLoose(this, gameplayUI, roundProcess),
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
        END_GAME,
        PLAYER_LOST,
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
            
            BackgroundMusicController.Instance.ActivateChooseSpellClip();
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
            UnsubscribeFromEvents();
            
            _gameStateMachine.ChangeState(TypeState.PLAYER_LOST);
        }

        private void EndingGame()
        {
            UnsubscribeFromEvents();

            _gameStateMachine.ChangeState(TypeState.END_GAME);
        }

        private void FinishTheRound()
        {
            UnsubscribeFromEvents();
            
            _gameStateMachine.ChangeState(TypeState.END_ROUND);
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
            _gameplayUI.OnContinuePlay = () => _gameStateMachine.ChangeState(TypeState.CHANGE_ABILITY);
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
            _gameplayUI.OnContinuePlay = () => _gameStateMachine.ChangeState(TypeState.CHANGE_ABILITY);
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
            _gameplayUI.OnContinuePlay = () => _gameStateMachine.ChangeState(TypeState.CHANGE_ABILITY);
        }
        
        public void Exit()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using Rebel_Mage.Enemy;
using Rebel_Mage.UI.Gameplay;
using Rebel_Mage.UI.Spell_Window;

namespace Rebel_Mage.Infrastructure 
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        
        private IExitableState _activeState;

        public GameStateMachine(IRoundProcess roundProcess, SpellWindowController spellWindowController, GameplayUI gameplayUI, IEnemySpawner enemySpawner, IFactoryActors factoryActors)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                // [typeof(BootstrapGame)] = new BootstrapGame(this),
                [typeof(StartGame)] = new StartGame(this, enemySpawner, factoryActors),
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

        // public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        // {
        //     TState state = ChangeState<TState>();
        //     state.Enter(payload);
        // }        
        
        // public void Enter<TState, TPayload1, TPayload2>(TPayload1 payload1, TPayload2 payload2) where TState : class, IPayloadState<TPayload1, TPayload2>
        // {
        //     TState state = ChangeState<TState>();
        //     state.Enter(payload1, payload2);
        // }
        
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

    // public interface IPayloadState<TPayload> : IExitableState
    // {
    //     public void Enter(TPayload payload);
    // }
    //
    // public interface IPayloadState<TPayload1, TPayload2> : IExitableState
    // {
    //     public void Enter(TPayload1 payload, TPayload2 payload2);
    // }

    // public class PreparingGameScene : IPayloadState<Action>
    // {
    //     private GameStateMachine _gameStateMachine;
    //     
    //     public PreparingGameScene(GameStateMachine gameStateMachine)
    //     {
    //         _gameStateMachine = gameStateMachine;
    //     }
    //     
    //     public void Enter(Action payload)
    //     {
    //     }
    //     
    //     public void Exit()
    //     {
    //     }
    // }

    // public class LoadSceneState : IPayloadState<string, Action>
    // {
    //     private readonly LoadingCurtains _loadingCurtains;
    //
    //     public LoadSceneState(IEnemySpawner loadingCurtains)
    //     {
    //         _loadingCurtains = loadingCurtains;
    //     }
    //
    //     private readonly CompositeDisposable _disposable = new();
    //
    //     public void Enter(string sceneName, Action onLoadedScene)
    //     {
    //         _loadingCurtains.Show();
    //         var waitNextScene = SceneManager.LoadSceneAsync(sceneName);
    //         StringBuilder progressText = new StringBuilder("0%");
    //
    //         Observable
    //             .EveryUpdate()
    //             .Subscribe(_ =>
    //             {
    //                 progressText.Insert(0, waitNextScene.progress);
    //                 progressText.Append("%");
    //                 _loadingCurtains.UpdateProgressText(progressText.ToString());
    //                 
    //                 if (waitNextScene.isDone)
    //                 {
    //                     _loadingCurtains.Hide();
    //                     onLoadedScene?.Invoke();
    //                 }
    //             })
    //             .AddTo(_disposable);
    //     }
    //     public void Exit()
    //     {
    //     }
    // }

    // public class BootstrapGame : IState
    // {
    //     private readonly GameStateMachine _gameStateMachine;
    //     
    //     public BootstrapGame(GameStateMachine gameStateMachine)
    //     {
    //         _gameStateMachine = gameStateMachine;
    //     }
    //     
    //     public void Enter()
    //     {
    //         _gameStateMachine.Enter<LoadSceneState, string, Action>("Gameplay", () =>
    //         {
    //             _gameStateMachine.Enter<StartGame>();
    //         });
    //     }
    //     
    //     public void Exit()
    //     {
    //     }
    // }
    
    public class StartGame : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IEnemySpawner _enemySpawner;
        private readonly IFactoryActors _factoryActors;

        public StartGame(GameStateMachine gameStateMachine, IEnemySpawner enemySpawner, IFactoryActors factoryActors)
        {
            _gameStateMachine = gameStateMachine;
            _enemySpawner = enemySpawner;
            _factoryActors = factoryActors;
        }
        
        public void Enter()
        {
            _factoryActors.InitFactoryActors(InitFactoryActorsEnded);
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
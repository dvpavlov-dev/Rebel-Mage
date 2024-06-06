using PushItOut.UI.Gameplay;
using PushItOut.UI.Spell_Window;
using UnityEngine;
using Zenject;

namespace Vanguard_Drone.Infrastructure
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private RoundProcess _roundProcess;
        [SerializeField] private GameplayUI _gameplayUI;
        [SerializeField] private SpellWindowController _spellWindowController;
        [SerializeField] private CameraManager _cameraManager;

        public override void InstallBindings()
        {
            BindRoundProcess();
            BindSpellWindow();
            BindGameplayUI();
            BindCameraManager();
        }
        private void BindCameraManager()
        {
            Container
                .Bind<CameraManager>()
                .FromInstance(_cameraManager)
                .AsSingle();
            
            _cameraManager.InitCameras();
        }

        private void BindRoundProcess()
        {
            Container
                .Bind<RoundProcess>()
                .FromInstance(_roundProcess)
                .AsSingle();
        }

        private void BindSpellWindow()
        {
            Container
                .Bind<SpellWindowController>()
                .FromInstance(_spellWindowController)
                .AsSingle();
        }
        
        private void BindGameplayUI()
        {
            Container
                .Bind<GameplayUI>()
                .FromInstance(_gameplayUI)
                .AsSingle();
        }
    }
}

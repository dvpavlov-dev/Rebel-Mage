using Rebel_Mage.Spell_system;
using Rebel_Mage.UI.Gameplay;
using Rebel_Mage.UI.Spell_Window;
using UnityEngine;
using Vanguard_Drone.Enemy;
using Zenject;

namespace Vanguard_Drone.Infrastructure
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [SerializeField] private RoundProcess _roundProcess;
        [SerializeField] private GameplayUI _gameplayUI;
        [SerializeField] private SpellWindowController _spellWindowController;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private EnemySpawner _enemySpawner;
        
        public override void InstallBindings()
        {
            BindRoundProcess();
            BindSpellWindow();
            BindGameplayUI();
            BindCameraManager();
            BindEnemySpawner();
        }

        private void BindEnemySpawner()
        {
            Container
                .Bind<IEnemySpawner>()
                .FromInstance(_enemySpawner)
                .AsSingle();
        }
        
        private void BindCameraManager()
        {
            Container
                .Bind<CameraManager>()
                .FromInstance(_cameraManager)
                .AsSingle();
        }

        private void BindRoundProcess()
        {
            Container
                .Bind<IRoundProcess>()
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

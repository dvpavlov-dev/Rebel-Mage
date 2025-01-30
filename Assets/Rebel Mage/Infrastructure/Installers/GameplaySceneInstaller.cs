using Rebel_Mage.Enemy;
using Rebel_Mage.UI.Gameplay;
using Rebel_Mage.UI.Spell_Window;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Rebel_Mage.Infrastructure
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [FormerlySerializedAs("m_RoundProcess")]
        [SerializeField] private RoundProcess _roundProcess;
        [FormerlySerializedAs("m_GameplayUI")]
        [SerializeField] private GameplayUI _gameplayUI;
        [FormerlySerializedAs("m_SpellWindowController")]
        [SerializeField] private SpellWindowController _spellWindowController;
        [FormerlySerializedAs("m_CameraManager")]
        [SerializeField] private CameraManager _cameraManager;
        [FormerlySerializedAs("m_EnemySpawner")]
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

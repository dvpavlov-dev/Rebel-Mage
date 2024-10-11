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
        [FormerlySerializedAs("_roundProcess")]
        [SerializeField] private RoundProcess m_RoundProcess;
        [FormerlySerializedAs("_gameplayUI")]
        [SerializeField] private GameplayUI m_GameplayUI;
        [FormerlySerializedAs("_spellWindowController")]
        [SerializeField] private SpellWindowController m_SpellWindowController;
        [FormerlySerializedAs("_cameraManager")]
        [SerializeField] private CameraManager m_CameraManager;
        [FormerlySerializedAs("_enemySpawner")]
        [SerializeField] private EnemySpawner m_EnemySpawner;
        
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
                .FromInstance(m_EnemySpawner)
                .AsSingle();
        }
        
        private void BindCameraManager()
        {
            Container
                .Bind<CameraManager>()
                .FromInstance(m_CameraManager)
                .AsSingle();
        }

        private void BindRoundProcess()
        {
            Container
                .Bind<IRoundProcess>()
                .FromInstance(m_RoundProcess)
                .AsSingle();
        }

        private void BindSpellWindow()
        {
            Container
                .Bind<SpellWindowController>()
                .FromInstance(m_SpellWindowController)
                .AsSingle();
        }
        
        private void BindGameplayUI()
        {
            Container
                .Bind<GameplayUI>()
                .FromInstance(m_GameplayUI)
                .AsSingle();
        }
    }
}

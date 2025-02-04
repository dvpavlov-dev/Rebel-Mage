using Rebel_Mage.Spell_system;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Infrastructure
{
    public class GlobalInstallers : MonoInstaller
    {
        [SerializeField] private Spells _spells;
        [SerializeField] private Configs _configs;
        [SerializeField] private Prefabs _prefabs;

        private IFactorySpells _factorySpells;
        private IUIFactory _uIFactory;

        public override void InstallBindings()
        {
            _factorySpells = new FactorySpells();
            _uIFactory = new UIFactory(_prefabs);
            
            BindSpells();
            BindConfigs();
            BindPrefabs();
            BindFactories();
            BindLoadingSceneService();
        }

        private void BindLoadingSceneService()
        {
            Container
                .Bind<ILoadingSceneService>()
                .FromInstance(new LoadingSceneServiceService(_uIFactory))
                .AsSingle();
        }

        private void BindPrefabs()
        {
            Container
                .Bind<Prefabs>()
                .FromInstance(_prefabs)
                .AsSingle();
        }
        private void BindConfigs()
        {
            Container
                .Bind<Configs>()
                .FromInstance(_configs)
                .AsSingle();
        }
        private void BindSpells()
        {
            Container
                .Bind<Spells>()
                .FromInstance(_spells)
                .AsSingle();

            _spells.Constructor(_factorySpells);
        }

        private void BindFactories()
        {
            Container
                .Bind<IFactoryActors>()
                .FromInstance(new FactoryActors(_prefabs, _configs, _uIFactory))
                .AsSingle();

            Container
                .Bind<IFactorySpells>()
                .FromInstance(_factorySpells)
                .AsSingle();
            
            Container
                .Bind<IUIFactory>()
                .FromInstance(new UIFactory(_prefabs))
                .AsSingle();
        }
    }
}

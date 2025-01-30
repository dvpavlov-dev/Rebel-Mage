using Rebel_Mage.Spell_system;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Infrastructure
{
    public class GlobalInstallers : MonoInstaller
    {
        [SerializeField] private Spells _spells;
        [SerializeField] private Configs.Configs _configs;
        [SerializeField] private Prefabs _prefabs;

        private readonly IFactorySpells _factorySpells = new FactorySpells();

        public override void InstallBindings()
        {
            BindSpells();
            BindConfigs();
            BindPrefabs();
            BindFactories();
            BindLoadingSceneService();
        }
        
        private void BindLoadingSceneService()
        {
            Container
                .Bind<ILoadingScene>()
                .FromInstance(new LoadingSceneService(_prefabs.LoadingCurtainsPref))
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
                .Bind<Configs.Configs>()
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
                .FromInstance(new FactoryActors(_prefabs, _configs))
                .AsSingle();

            Container
                .Bind<IFactorySpells>()
                .FromInstance(_factorySpells)
                .AsSingle();
        }
    }
}

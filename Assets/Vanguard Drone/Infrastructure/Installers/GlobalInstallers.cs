using PushItOut.Spell_system;
using UnityEngine;
using Zenject;

namespace Vanguard_Drone.Infrastructure
{
    public class GlobalInstallers : MonoInstaller
    {
        [SerializeField] private Spells _spells;
        [SerializeField] private Configs _configs;
        [SerializeField] private Prefabs _prefabs;

        public override void InstallBindings()
        {
            BindSpells();
            BindConfigs();
            BindPrefabs();
            BindFactories();
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
        }

        private void BindFactories()
        {
            Container
                .Bind<Factory>()
                .FromInstance(new Factory(Container, _prefabs, _configs))
                .AsSingle();
        }
    }
}

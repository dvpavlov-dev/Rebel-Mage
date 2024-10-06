using Rebel_Mage.Infrastructure;
using Rebel_Mage.Spell_system;
using UnityEngine;
using Zenject;

namespace Vanguard_Drone.Infrastructure
{
    public class GlobalInstallers : MonoInstaller
    {
        [SerializeField] private Spells _spells;
        [SerializeField] private Configs _configs;
        [SerializeField] private Prefabs _prefabs;

        readonly IFactorySpells m_FactorySpells = new FactorySpells();
        
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
            
            _spells.Constructor(m_FactorySpells);
        }

        private void BindFactories()
        {
            Container
                .Bind<IFactoryActors>()
                .FromInstance(new FactoryActors(_prefabs, _configs))
                .AsSingle();
            
            Container
                .Bind<IFactorySpells>()
                .FromInstance(m_FactorySpells)
                .AsSingle();
        }
    }
}

using Rebel_Mage.Spell_system;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Rebel_Mage.Infrastructure
{
    public class GlobalInstallers : MonoInstaller
    {
        [FormerlySerializedAs("_spells")]
        [SerializeField] private Spells m_Spells;
        [FormerlySerializedAs("_configs")]
        [SerializeField] private Configs.Configs m_Configs;
        [FormerlySerializedAs("_prefabs")]
        [SerializeField] private Prefabs m_Prefabs;

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
                .FromInstance(m_Prefabs)
                .AsSingle();
        }
        private void BindConfigs()
        {
            Container
                .Bind<Configs.Configs>()
                .FromInstance(m_Configs)
                .AsSingle();
        }
        private void BindSpells()
        {
            Container
                .Bind<Spells>()
                .FromInstance(m_Spells)
                .AsSingle();

            m_Spells.Constructor(m_FactorySpells);
        }

        private void BindFactories()
        {
            Container
                .Bind<IFactoryActors>()
                .FromInstance(new FactoryActors(m_Prefabs, m_Configs))
                .AsSingle();

            Container
                .Bind<IFactorySpells>()
                .FromInstance(m_FactorySpells)
                .AsSingle();
        }
    }
}

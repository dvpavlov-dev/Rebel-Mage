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

        private ISpellsFactory _spellsFactory;
        private IUIFactory _uIFactory;

        public override void InstallBindings()
        {
            _spellsFactory = new SpellsFactory();
            _uIFactory = new UIFactory(_prefabs);
            
            BindSpells();
            BindConfigs();
            BindPrefabs();
            BindFactories();
            BindLoadingSceneService();
        }

        private void BindLoadingSceneService()
        {
            Container.BindInterfacesTo<LoadingSceneServiceService>().AsSingle().WithArguments(_uIFactory).NonLazy();
        }

        private void BindPrefabs()
        {
            Container.BindInstance(_prefabs).AsSingle().NonLazy();
        }
        
        private void BindConfigs()
        {
            Container.BindInstance(_configs).AsSingle().NonLazy();
        }
        
        private void BindSpells()
        {
            Container.BindInstance(_spells).AsSingle().WithArguments(_spellsFactory).NonLazy();
        }

        private void BindFactories()
        {
            Container.BindInterfacesTo<ActorsFactory>().AsSingle().WithArguments(_prefabs, _configs, _uIFactory).NonLazy();
            Container.BindInstance(_spellsFactory).AsSingle().NonLazy();
            Container.BindInterfacesTo<UIFactory>().AsSingle().WithArguments(_prefabs).NonLazy();
        }
    }
}

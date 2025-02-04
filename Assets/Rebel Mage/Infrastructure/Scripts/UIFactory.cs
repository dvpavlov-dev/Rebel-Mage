using Rebel_Mage.UI;
using UnityEngine;

namespace Rebel_Mage.Infrastructure
{
    public class UIFactory : IUIFactory
    {
        private readonly Prefabs _prefabs;
        private LoadingCurtains _loadingCurtains;

        public UIFactory(Prefabs prefabs) 
        {
            _prefabs = prefabs;
        }

        public LoadingCurtains CreateLoadingCurtains()
        {
            _loadingCurtains ??= GameObject.Instantiate(_prefabs.LoadingCurtainsPref).GetComponent<LoadingCurtains>();
            _loadingCurtains.Init();
        
            return _loadingCurtains;
        }
    }
    
    public interface IUIFactory
    {
        LoadingCurtains CreateLoadingCurtains();
    }
}
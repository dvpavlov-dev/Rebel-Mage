using System;
using UnityEngine.SceneManagement;
using R3;
using Rebel_Mage.UI;
using UnityEngine;

namespace Rebel_Mage.Infrastructure
{
    public class LoadingSceneServiceService : ILoadingSceneService
    {
        public Action OnSceneLoaded { get; set; }
    
        private readonly LoadingCurtains _loadingCurtains;
        private readonly CompositeDisposable _disposable = new();
    
        public LoadingSceneServiceService(IUIFactory uiFactory)
        {
            _loadingCurtains = uiFactory.CreateLoadingCurtains();
        
            Application.quitting += OnGameQuit;
        }
    
        public void LoadScene(string sceneName)
        {
            _loadingCurtains.Show();
            var waitNextScene = SceneManager.LoadSceneAsync(sceneName);

            Observable
                .EveryUpdate()
                .Subscribe(_ =>
                {
                    _loadingCurtains.UpdateProgress(waitNextScene.progress);
                    _loadingCurtains.UpdateDescription("Загрузка сцены, подождите...");
        
                    if (waitNextScene.isDone)
                    {
                        _loadingCurtains.Hide();
                        OnSceneLoaded?.Invoke();
                        _disposable.Dispose();
                    }
                })
                .AddTo(_disposable);
        }
    
        private void OnGameQuit()
        {
            _disposable.Dispose();
        }
    }
    
    public interface ILoadingSceneService
    {
        Action OnSceneLoaded { get; set;  }
    
        void LoadScene(string sceneName);
    }
}
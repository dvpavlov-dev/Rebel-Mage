using System;
using UnityEngine.SceneManagement;
using R3;
using UnityEngine;

public class LoadingSceneService : ILoadingScene
{
    private readonly LoadingCurtains _loadingCurtains;
    private readonly CompositeDisposable _disposable = new();
    
    public LoadingSceneService(IUIFactory uiFactory)
    {
        _loadingCurtains = uiFactory.CreateLoadingCurtains();
        
        Application.quitting += OnGameQuit;
    }
    
    public void LoadScene(string sceneName, Action onLoadingScene)
    {
        _loadingCurtains.Show();
        var waitNextScene = SceneManager.LoadSceneAsync(sceneName);

        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                _loadingCurtains.UpdateProgress(waitNextScene.progress);
        
                if (waitNextScene.isDone)
                {
                    _loadingCurtains.Hide();
                    onLoadingScene?.Invoke();
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

public interface ILoadingScene
{
    public void LoadScene(string sceneName, Action onLoadingScene);
}
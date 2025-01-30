using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

public class LoadingSceneService : ILoadingScene
{
    private readonly LoadingCurtains _loadingCurtains;
    private readonly CompositeDisposable _disposable = new();
    
    public LoadingSceneService(GameObject loadingCurtainsPref)
    {
        _loadingCurtains = GameObject.Instantiate(loadingCurtainsPref).GetComponent<LoadingCurtains>();
    }

    public void LoadScene(string sceneName, Action onLoadingScene)
    {
        _loadingCurtains.Show();
        var waitNextScene = SceneManager.LoadSceneAsync(sceneName);
        StringBuilder progressText = new("0%");

        Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                progressText.Clear();
                progressText.Insert(0, waitNextScene.progress);
                progressText.Append("%");
                _loadingCurtains.UpdateProgressText(progressText.ToString());
        
                if (waitNextScene.isDone)
                {
                    _loadingCurtains.Hide();
                    onLoadingScene?.Invoke();
                }
            })
            .AddTo(_disposable);
    }
}

public interface ILoadingScene
{
    public void LoadScene(string sceneName, Action onLoadingScene);
}

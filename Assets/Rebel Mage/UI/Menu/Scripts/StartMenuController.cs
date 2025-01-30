using UnityEngine;
using Zenject;

public class StartMenuController : MonoBehaviour
{
    private ILoadingScene _loadingSceneService;
    
    [Inject]
    private void Constructor(ILoadingScene loadingSceneService)
    {
        _loadingSceneService = loadingSceneService;
    }
    
    public void OnSelectedStart()
    {
        Invoke(nameof(LoadGamePlayScene), 0.2f); 
    }

    private void LoadGamePlayScene()
    {
        // SceneManager.LoadSceneAsync(1);
        _loadingSceneService.LoadScene("Gameplay", null);
    }

    public void OnSelectedExit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}

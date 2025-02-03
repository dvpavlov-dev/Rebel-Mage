using UnityEngine;
using Zenject;

public class StartMenuController : MonoBehaviour
{
    private ILoadingSceneService _loadingSceneServiceService;
    
    [Inject]
    private void Constructor(ILoadingSceneService loadingSceneServiceService)
    {
        _loadingSceneServiceService = loadingSceneServiceService;
    }
    
    public void OnSelectedStart()
    {
        Invoke(nameof(LoadGamePlayScene), 0.2f); 
    }

    private void LoadGamePlayScene()
    {
        _loadingSceneServiceService.LoadScene("Gameplay");
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

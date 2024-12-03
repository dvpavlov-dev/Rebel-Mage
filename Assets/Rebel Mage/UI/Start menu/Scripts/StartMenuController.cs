using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnSelectedStart()
    {
        Invoke(nameof(LoadGamePlayScene), 0.2f); 
    }

    private void LoadGamePlayScene()
    {
        SceneManager.LoadSceneAsync(1);
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

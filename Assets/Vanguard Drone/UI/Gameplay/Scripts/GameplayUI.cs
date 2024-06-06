using UnityEngine;
using UnityEngine.SceneManagement;

namespace PushItOut.UI.Gameplay
{
    public class GameplayUI : MonoBehaviour
    {
        public GameObject Menu;
        public GameObject LevelEnd;
        public GameObject SpellsPanel;

        public void LevelOver()
        {
            Menu.SetActive(false);
            LevelEnd.SetActive(true);
        }

        public void OnClickContinue()
        {
            Menu.SetActive(false);
        }
        
        public void OnClickExit()
        {
            SceneManager.LoadScene("Start scene");
        }
    }
}

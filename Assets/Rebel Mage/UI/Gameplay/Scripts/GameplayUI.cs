using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rebel_Mage.UI
{
    public class GameplayUI : MonoBehaviour
    {
        public GameObject Menu;
        public GameObject RoundEnd;
        public GameObject SpellsPanel;

        public Action OnContinuePlay;

        public void RoundOver(bool isVictory, int currentPointsCollected)
        {
            Menu.SetActive(false);
            RoundEnd.SetActive(true);

            RoundEndView roundEndView = RoundEnd.GetComponent<RoundEndView>();
            roundEndView.SetTitle(isVictory);
            roundEndView.ShowCurrentPoints(currentPointsCollected);
        }

        public void OpenMenu()
        {
            Menu.SetActive(true);
        }

        public void OnClickContinue()
        {
            Menu.SetActive(false);
        }

        public void OnClickContinueRound()
        {
            RoundEnd.SetActive(false);
            OnContinuePlay?.Invoke();
        }
        
        public void OnClickExit()
        {
            SceneManager.LoadScene(0);
        }
    }
}

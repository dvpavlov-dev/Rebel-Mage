using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PushItOut.UI.Gameplay
{
    public class GameplayUI : MonoBehaviour
    {
        public GameObject Menu;
        public GameObject RoundEnd;
        public GameObject SpellsPanel;

        public Action OnContinuePlay;

        public void RoundOver(string textTitle, int currentPointsCollected)
        {
            Menu.SetActive(false);
            RoundEnd.SetActive(true);

            RoundEndView roundEndView = RoundEnd.GetComponent<RoundEndView>();
            roundEndView.SetTextTitle(textTitle);
            roundEndView.ShowCurrentPoints(currentPointsCollected);
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
            SceneManager.LoadScene("Start scene");
        }
    }
}
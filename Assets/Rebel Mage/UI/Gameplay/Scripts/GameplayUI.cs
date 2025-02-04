using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rebel_Mage.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _roundEnd;
        [SerializeField] private GameObject _spellsPanel;

        public GameObject Menu => _menu;
        public GameObject SpellsPanel => _spellsPanel;
        
        public Action OnContinuePlay { get; set; }

        public void RoundOver(bool isVictory, int currentPointsCollected)
        {
            _menu.SetActive(false);
            _roundEnd.SetActive(true);

            RoundEndView roundEndView = _roundEnd.GetComponent<RoundEndView>();
            roundEndView.SetTitle(isVictory);
            roundEndView.ShowCurrentPoints(currentPointsCollected);
        }

        public void OnClickContinue()
        {
            _menu.SetActive(false);
        }

        public void OnClickContinueRound()
        {
            _roundEnd.SetActive(false);
            OnContinuePlay?.Invoke();
        }
        
        public void OnClickExit()
        {
            SceneManager.LoadScene(0);
        }
    }
}

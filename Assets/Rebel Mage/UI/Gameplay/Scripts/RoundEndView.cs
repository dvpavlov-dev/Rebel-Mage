using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rebel_Mage.UI
{
    public class RoundEndView : MonoBehaviour
    {
        [FormerlySerializedAs("VictoryImage")]
        [SerializeField] private GameObject _victoryImage;
        [FormerlySerializedAs("DefeatImage")]
        [SerializeField] private GameObject _defeatImage;
        [FormerlySerializedAs("ScoreText")]
        [SerializeField] private TextMeshProUGUI _scoreText;
    
        public void SetTitle(bool isVictory)
        {
            _victoryImage.SetActive(false);
            _defeatImage.SetActive(false);
        
            if (isVictory)
            {
                _victoryImage.SetActive(true);
            }
            else
            {
                _defeatImage.SetActive(true);
            }
        }
    
        public void ShowCurrentPoints(int currentPoints)
        {
            _scoreText.text = currentPoints.ToString();
        }
    }
}
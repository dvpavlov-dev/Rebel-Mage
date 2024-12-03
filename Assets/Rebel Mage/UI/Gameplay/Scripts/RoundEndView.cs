using TMPro;
using UnityEngine;

public class RoundEndView : MonoBehaviour
{
    [SerializeField] private GameObject VictoryImage;
    [SerializeField] private GameObject DefeatImage;
    [SerializeField] private TextMeshProUGUI ScoreText;
    
    public void SetTitle(bool isVictory)
    {
        VictoryImage.SetActive(false);
        DefeatImage.SetActive(false);
        
        if (isVictory)
        {
            VictoryImage.SetActive(true);
        }
        else
        {
            DefeatImage.SetActive(true);
        }
    }
    
    public void ShowCurrentPoints(int currentPoints)
    {
        ScoreText.text = currentPoints.ToString();
    }
}

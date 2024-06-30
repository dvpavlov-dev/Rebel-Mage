using TMPro;
using UnityEngine;

public class RoundEndView : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;

    public void SetTextTitle(string text)
    {
        Title.text = text;
    }
    
    public void ShowCurrentPoints(int currentPoints)
    {
        Description.text = $"Points collected: {currentPoints}";
    }
}

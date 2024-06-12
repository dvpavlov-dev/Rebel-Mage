using TMPro;
using UnityEngine;

public class RoundEndView : MonoBehaviour
{
    public TextMeshProUGUI Description;

    public void ShowCurrentPoints(int currentPoints)
    {
        Description.text = $"Points collected: {currentPoints}";
    }
}

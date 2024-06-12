using TMPro;
using UnityEngine;
using Vanguard_Drone.Infrastructure;

public class RoundEndView : MonoBehaviour
{
    public TextMeshProUGUI Description;
    
    private RoundProcess _roundProcess;

    private void Constructor(RoundProcess roundProcess)
    {
        _roundProcess = roundProcess;
    }
    
    
}

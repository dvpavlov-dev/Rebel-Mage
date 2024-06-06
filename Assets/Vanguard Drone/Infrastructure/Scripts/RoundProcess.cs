using UnityEngine;

namespace Vanguard_Drone.Infrastructure
{
    public class RoundProcess : MonoBehaviour
    {
        public bool IsRoundInProgress { get; set; }

        public void StartRound()
        {
            IsRoundInProgress = true;
        }
    }
}

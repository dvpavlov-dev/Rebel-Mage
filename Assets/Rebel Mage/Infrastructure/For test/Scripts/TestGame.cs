using UnityEngine;
using Vanguard_Drone.Player;

public class TestGame : MonoBehaviour
{
    public GameObject TestPlayer;
    
    void Start()
    {
        GameObject testPlayer = Instantiate(TestPlayer, transform.position, Quaternion.identity);
        testPlayer.GetComponent<PlayerMoveController>().Init(10);
    }
}

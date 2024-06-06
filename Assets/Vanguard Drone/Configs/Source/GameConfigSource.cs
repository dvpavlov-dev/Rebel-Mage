using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
public class GameConfigSource : ScriptableObject
{
    public float RoundTime;
    public int RoundsInGame;
}

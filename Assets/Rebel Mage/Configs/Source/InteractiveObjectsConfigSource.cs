using UnityEngine;

[CreateAssetMenu(fileName = "InteractiveObjectsConfig", menuName = "Configs/Interactive Objects Config")]
public class InteractiveObjectsConfigSource : ScriptableObject
{
    public GameObject InteractObjPref;
    public int ObjectsCount;
    public float IntervalSpawn;
}

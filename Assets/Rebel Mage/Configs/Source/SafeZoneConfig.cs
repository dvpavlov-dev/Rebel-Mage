using UnityEngine;

[CreateAssetMenu(fileName = "SafeZoneConfig", menuName = "Configs/SafeZoneConfig")]
public class SafeZoneConfig : ScriptableObject
{
    public float DamageOutZone;
    public float IntervalDamage;
    public float ReduceZoneSpeed;
    public float MinimumZoneSize;
}

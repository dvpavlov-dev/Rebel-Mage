using System;
using UnityEngine;

namespace Rebel_Mage.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
    public class EnemyConfigSource : ScriptableObject
    {
        public GeneralEnemyParameters BaseEnemy;
        public GeneralEnemyParameters MeleeEnemy;
    }

    [Serializable]
    public class GeneralEnemyParameters
    {
        public EnemyType EnemyType;
        public GameObject EnemyPref;
        
        [Header("Values")]
        public float Hp;
        public float MoveSpeed;
        public float Damage;
        public int Points;
        public float StoppingDistance;
    }
}

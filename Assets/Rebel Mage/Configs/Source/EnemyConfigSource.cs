using System;
using UnityEngine;

namespace Rebel_Mage.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
    public class EnemyConfigSource : ScriptableObject
    {
        public BaseEnemyParameters BaseEnemy;
        public MeleeEnemyParameters MeleeEnemy;
    }

    [Serializable]
    public struct BaseEnemyParameters
    {
        public float Hp;
        public float MoveSpeed;
        public float Damage;
        public int Points;
        public float StoppingDistance;
    }

    [Serializable]
    public struct MeleeEnemyParameters
    {
        public float Hp;
        public float MoveSpeed;
        public float Damage;
        public int Points;
        public float StoppingDistance;
    }
}

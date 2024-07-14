using UnityEngine;

namespace PushItOut.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
    public class EnemyConfigSource : ScriptableObject
    {
        [Header("Base enemy")]
        public float BaseEnemy_Hp;
        public float BaseEnemy_MoveSpeed;
        public float BaseEnemy_Damage;
        public int BaseEnemy_Points;
    }
}

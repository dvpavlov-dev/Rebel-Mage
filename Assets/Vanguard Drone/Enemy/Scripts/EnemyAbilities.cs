using UnityEngine;

namespace Vanguard_Drone.Enemy
{
    public class EnemyAbilities : MonoBehaviour
    {
        protected float Damage;
        
        public void SetupEnemyAbilities(float damage)
        {
            Damage = damage;
        }
    }
}

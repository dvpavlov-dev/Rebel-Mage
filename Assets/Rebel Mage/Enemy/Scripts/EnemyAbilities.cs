using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class EnemyAbilities : MonoBehaviour
    {
        protected float Damage;
        protected Enemy Enemy;
        protected GameObject Target;

        public void SetupEnemyAbilities(float damage, GameObject target, Enemy meleeEnemy)
        {
            Damage = damage;
            Target = target;
            Enemy = meleeEnemy;
        }
    }
}

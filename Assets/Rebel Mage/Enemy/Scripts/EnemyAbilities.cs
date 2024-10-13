using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class EnemyAbilities : MonoBehaviour
    {
        protected float Damage;
        protected Enemy EnemyController;
        protected GameObject Target;
        protected bool IsEnemyAbilitiesSetup;

        public void SetupEnemyAbilities(float damage, GameObject target, Enemy meleeEnemy)
        {
            Damage = damage;
            Target = target;
            EnemyController = meleeEnemy;

            IsEnemyAbilitiesSetup = true;
        }
    }
}

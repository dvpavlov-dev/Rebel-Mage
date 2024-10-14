using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class EnemyAbilities<T> : MonoBehaviour where T : EnemyView
    {
        protected float Damage;
        protected Enemy<T> EnemyController;
        protected T EnemyView;
        protected GameObject Target;
        protected bool IsEnemyAbilitiesSetup;

        public void SetupEnemyAbilities(float damage, GameObject target, T enemyView, Enemy<T> enemyController)
        {
            Damage = damage;
            Target = target;
            EnemyController = enemyController;
            EnemyView = enemyView;

            IsEnemyAbilitiesSetup = true;
        }
    }
}

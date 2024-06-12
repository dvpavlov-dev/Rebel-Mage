using UnityEngine;

namespace Vanguard_Drone.Enemy.Scripts
{
    public class BaseEnemy : Enemy
    {
        public override void InitEnemy(Infrastructure.Configs configs, GameObject target)
        {
            base.InitEnemy(configs, target);

            _pointsForEnemy = configs.EnemyConfig.BaseEnemy_Points;

            EnemyAI = gameObject.AddComponent<BaseEnemyAI>();
            EnemyAbilities = gameObject.AddComponent<BaseEnemyAbilities>();
            
            EnemyAI.SetupEnemyAI(configs.EnemyConfig.BaseEnemy_MoveSpeed, Target);
            EnemyAbilities.SetupEnemyAbilities(configs.EnemyConfig.BaseEnemy_Damage);
        }
    }
}

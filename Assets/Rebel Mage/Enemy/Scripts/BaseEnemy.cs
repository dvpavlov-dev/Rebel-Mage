using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class BaseEnemy : Enemy
    {
        public override void InitEnemy(Configs.Configs configs, GameObject target)
        {
            base.InitEnemy(configs, target);
            
            PointsForEnemy = configs.EnemyConfig.BaseEnemy.Points;

            EnemyAI = gameObject.AddComponent<EnemyAI>();
            EnemyAbilities = gameObject.AddComponent<BaseEnemyAbilities>();

            GetComponent<DamageController>().InitHealthPoints(configs.EnemyConfig.BaseEnemy.Hp);
            EnemyAI.SetupEnemyAI(configs.EnemyConfig.BaseEnemy.MoveSpeed, Target, configs.EnemyConfig.BaseEnemy.StoppingDistance, this);
            EnemyAbilities.SetupEnemyAbilities(configs.EnemyConfig.BaseEnemy.Damage, target, this);
        }
    }
}

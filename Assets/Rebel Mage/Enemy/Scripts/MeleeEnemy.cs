using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(MeleeEnemyAI), typeof(MeleeEnemyAbilities))]
    public class MeleeEnemy : Enemy
    {
        public override void InitEnemy(Configs.Configs configs, GameObject target)
        {
            PointsForEnemy = configs.EnemyConfig.MeleeEnemy.Points;

            EnemyAI = gameObject.GetComponent<MeleeEnemyAI>();
            EnemyAbilities = gameObject.GetComponent<MeleeEnemyAbilities>();


            GetComponent<DamageController>().InitHealthPoints(configs.EnemyConfig.MeleeEnemy.Hp);
            EnemyAI.SetupEnemyAI(configs.EnemyConfig.MeleeEnemy.MoveSpeed, target, configs.EnemyConfig.MeleeEnemy.StoppingDistance, this);
            EnemyAbilities.SetupEnemyAbilities(configs.EnemyConfig.MeleeEnemy.Damage, target, this);
            
            base.InitEnemy(configs, target);
        }
    }
}

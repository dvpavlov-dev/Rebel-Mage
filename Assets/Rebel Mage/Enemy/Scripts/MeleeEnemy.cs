using System;
using Rebel_Mage.Configs;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(MeleeEnemyAI), typeof(MeleeEnemyAbilities))]
    public class MeleeEnemy : Enemy<MeleeEnemyView>
    {
        public override void InitEnemy(Configs.Configs configs, GameObject target, Action onDead)
        {
            MeleeEnemyParameters config = configs.EnemyConfig.MeleeEnemy;
            
            PointsForEnemy = config.Points;

            EnemyAI = gameObject.GetComponent<MeleeEnemyAI>();
            EnemyAbilities = gameObject.GetComponent<MeleeEnemyAbilities>();

            base.InitEnemy(configs, target, onDead);

            DmgController.InitHealthPoints(config.Hp);
            EnemyAI.SetupEnemyAI(config.MoveSpeed, target, config.StoppingDistance, EnemyView, this);
            EnemyAbilities.SetupEnemyAbilities(config.Damage, target, EnemyView, this);
            
            EnemySM.ChangeState<MoveState<MeleeEnemyView>>();
        }
    }
}

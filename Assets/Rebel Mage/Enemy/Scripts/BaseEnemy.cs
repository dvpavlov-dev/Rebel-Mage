using System;
using Rebel_Mage.Configs;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(BaseEnemyAI), typeof(BaseEnemyAbilities))]
    public class BaseEnemy : Enemy <BaseEnemyView>
    {
        public override void InitEnemy(Configs.Configs configs, GameObject target, Action onDead)
        {
            BaseEnemyParameters config = configs.EnemyConfig.BaseEnemy;
            
            PointsForEnemy = config.Points;

            EnemyAI = gameObject.GetComponent<BaseEnemyAI>();
            EnemyAbilities = gameObject.GetComponent<BaseEnemyAbilities>();
            
            base.InitEnemy(configs, target, onDead);

            DmgController.InitHealthPoints(config.Hp);
            EnemyAI.SetupEnemyAI(config.MoveSpeed, target, config.StoppingDistance, EnemyView, this);
            EnemyAbilities.SetupEnemyAbilities(config.Damage, target, EnemyView, this);

            SetMoveState();
        }
    }
}

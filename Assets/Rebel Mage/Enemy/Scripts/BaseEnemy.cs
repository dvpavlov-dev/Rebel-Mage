using System;
using Rebel_Mage.Infrastructure;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(BaseEnemyAI), typeof(BaseEnemyAbilities))]
    public class BaseEnemy : Enemy <BaseEnemyView>
    {
        public override void InitEnemy(Infrastructure.Configs configs, GameObject target, Action onDead, ActorsFactory actorsFactory)
        {
            _Config = configs.EnemyConfig.BaseEnemy;
            
            PointsForEnemy = _Config.Points;

            _EnemyAI = gameObject.GetComponent<BaseEnemyAI>();
            _EnemyAbilities = gameObject.GetComponent<BaseEnemyAbilities>();
            
            base.InitEnemy(configs, target, onDead, actorsFactory);

            _DmgController.InitHealthPoints(_Config.Hp);
            _EnemyAI.SetupEnemyAI(_Config.MoveSpeed, target, _Config.StoppingDistance, EnemyView, this);
            _EnemyAbilities.SetupEnemyAbilities(_Config.Damage, target, EnemyView, this, _AudioSource);

            SetMoveState();
        }
    }
}

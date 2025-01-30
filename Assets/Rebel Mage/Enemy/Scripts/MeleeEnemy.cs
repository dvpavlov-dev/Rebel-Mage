using System;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(MeleeEnemyAI), typeof(MeleeEnemyAbilities))]
    public class MeleeEnemy : Enemy<MeleeEnemyView>
    {
        public override void InitEnemy(Configs.Configs configs, GameObject target, Action onDead)
        {
            _Config = configs.EnemyConfig.MeleeEnemy;
            
            PointsForEnemy = _Config.Points;

            _EnemyAI = gameObject.GetComponent<MeleeEnemyAI>();
            _EnemyAbilities = gameObject.GetComponent<MeleeEnemyAbilities>();

            base.InitEnemy(configs, target, onDead);

            _DmgController.InitHealthPoints(_Config.Hp);
            _EnemyAI.SetupEnemyAI(_Config.MoveSpeed, target, _Config.StoppingDistance, EnemyView, this);
            _EnemyAbilities.SetupEnemyAbilities(_Config.Damage, target, EnemyView, this, _AudioSource);

            SetMoveState();
        }
    }
}

using System;
using PushItOut.Spell_system;
using UnityEngine;

namespace Vanguard_Drone.Enemy
{
    [RequireComponent(typeof(DamageController))]
    public class Enemy : MonoBehaviour
    {
        public int _pointsForEnemy { get; protected set; }
        public Action OnDead;

        protected EnemyAI EnemyAI;
        protected EnemyAbilities EnemyAbilities;

        protected GameObject Target;


        public virtual void InitEnemy(Infrastructure.Configs configs, GameObject target)
        {
            Target = target;
            
            GetComponent<DamageController>().InitHealthPoints(configs.EnemyConfig.BaseEnemy_Hp);
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            
            OnDead?.Invoke();
        }
    }
}

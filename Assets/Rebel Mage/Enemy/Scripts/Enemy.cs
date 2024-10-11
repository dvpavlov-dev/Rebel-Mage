using System;
using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(DamageController))]
    public class Enemy : MonoBehaviour
    {
        public Action OnDead;
        
        protected EnemyAbilities EnemyAbilities;
        protected EnemyAI EnemyAI;
        protected GameObject Target;
        protected IDamage DamageController;
        
        public int PointsForEnemy { get; protected set; }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;

            OnDead?.Invoke();
        }

        public virtual void InitEnemy(Configs.Configs configs, GameObject target)
        {
            Target = target;

            DamageController = GetComponent<IDamage>();
        }
    }
}

using PushItOut.Spell_system;
using UnityEngine;

namespace Vanguard_Drone.Enemy
{
    [RequireComponent(typeof(DamageController))]
    public class Enemy : MonoBehaviour
    {
        protected EnemyAI EnemyAI;
        protected EnemyAbilities EnemyAbilities;
        
        protected GameObject Target;

        public virtual void InitEnemy(Infrastructure.Configs configs, GameObject target)
        {
            Target = target;
            
            GetComponent<DamageController>().InitHealthPoints(configs.EnemyConfig.BaseEnemy_Hp);
        }
    }
}

using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyAI : EnemyAI
    {
        public Animator Animator;
        
        private static readonly int MoveForward = Animator.StringToHash("MoveForward");

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            Animator.SetFloat(MoveForward, MoveCoefficient);
        }
    }
}

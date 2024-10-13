using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyAI : EnemyAI
    {
        private static readonly int MoveForward = Animator.StringToHash("MoveForward");

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!IsEnemySetup) return;

            EnemyController.AnimationController.SetFloat(MoveForward, MoveCoefficient);
        }
    }
}

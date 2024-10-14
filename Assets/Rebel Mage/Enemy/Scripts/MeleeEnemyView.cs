using System;
using System.Linq;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyView : EnemyView
    {
        public Action<string> OnEndAnimationAction;

        private Transform m_Parent;

        private static readonly int MoveForward = Animator.StringToHash("MoveForward");
        private static readonly int MutantPunch = Animator.StringToHash("Mutant Punch");

        public void OnEndAnimation(string animationName) // Used in animation events
        {
            OnEndAnimationAction?.Invoke(animationName);
        }

        public void StartMoveAnimation(float moveCoefficient)
        {
            AnimationController.SetFloat(MoveForward, moveCoefficient);
        }

        public void StartAttackAnimation()
        {
            AnimationController.SetTrigger(MutantPunch);
        }

        public override void ReactionOnExplosion(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            Rigidbody hitBone = Rigidbodies.OrderBy(rigidbody => Vector3.Distance(positionImpact, rigidbody.position)).First();
            hitBone.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
        }
    }
}

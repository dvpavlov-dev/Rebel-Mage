using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class MeleeEnemyView : MonoBehaviour
    {
        public Action<string> OnEndAnimationAction;
        
        private Animator m_AnimationController;
        private List<Rigidbody> m_Rigidbodies;
        private Transform m_Parent;
        
        private static readonly int MoveForward = Animator.StringToHash("MoveForward");
        private static readonly int MutantPunch = Animator.StringToHash("Mutant Punch");

        public void Init(Transform parent)
        {
            m_AnimationController = GetComponent<Animator>();
            m_Rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        }
        
        public void OnEndAnimation(string animationName) // Used in animation events
        {
            OnEndAnimationAction?.Invoke(animationName);
        }
        
        public void EnabledAnimator() => m_AnimationController.enabled = true;
        public void DisabledAnimator() => m_AnimationController.enabled = false;

        public void StartMoveAnimation(float moveCoefficient)
        {
            m_AnimationController.SetFloat(MoveForward, moveCoefficient);
        }

        public void StartAttackAnimation()
        {
            m_AnimationController.SetTrigger(MutantPunch);
        }

        public void ReactionOnExplosion(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            Rigidbody hitBone = m_Rigidbodies.OrderBy(rigidbody => Vector3.Distance(positionImpact, rigidbody.position)).First();
            hitBone.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
        }
        
        public void EnableRigidbody()
        {
            foreach (Rigidbody rb in m_Rigidbodies)
            {
                rb.isKinematic = false;
            }
        }

        public void DisableRigidbody()
        {
            foreach (Rigidbody rb in m_Rigidbodies)
            {
                rb.isKinematic = true;
            }
        }
    }
}

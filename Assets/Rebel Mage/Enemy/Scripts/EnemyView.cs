using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyView : MonoBehaviour
    {
        private void EnabledAnimator() => AnimationController.enabled = true;
        private void DisabledAnimator() => AnimationController.enabled = false;
        
        protected Animator AnimationController;
        protected List<Rigidbody> Rigidbodies;

        private bool m_IsRigidBodyEnabled;

        public virtual void Init(Transform parent)
        {
            AnimationController = GetComponent<Animator>();
            Rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        }

        public virtual void ReactionOnExplosion(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
        }

        public void EnableRigidbody()
        {
            DisabledAnimator();
            
            foreach (Rigidbody rb in Rigidbodies)
            {
                rb.isKinematic = false;
            }
        }

        public virtual void DisableRigidbody(Action onEndStandingUpAnimation)
        {
            foreach (Rigidbody rb in Rigidbodies)
            {
                rb.isKinematic = true;
            }
            
            EnabledAnimator();
        }
    }
}

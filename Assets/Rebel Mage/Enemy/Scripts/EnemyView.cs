using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyView : MonoBehaviour
    {

        protected Animator AnimationController;
        protected List<Rigidbody> Rigidbodies;
        public bool IsRigidBodyEnabled { get; protected set; }

        protected virtual void Update() {}

        protected void EnabledAnimator() => AnimationController.enabled = true;
        private void DisabledAnimator() => AnimationController.enabled = false;

        public virtual void Init(Transform parent)
        {
            AnimationController = GetComponent<Animator>();
            Rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        }

        public virtual void ReactionOnExplosion(Vector3 positionImpact, float maxDistance, float explosionForce) {}

        public void EnableRigidbody()
        {
            DisabledAnimator();

            foreach (Rigidbody rb in Rigidbodies)
            {
                rb.isKinematic = false;
            }

            IsRigidBodyEnabled = true;
        }

        public virtual void DisableRigidbody(Action onEndStandingUpAnimation = null)
        {
            foreach (Rigidbody rb in Rigidbodies)
            {
                rb.isKinematic = true;
            }

            EnabledAnimator();

            IsRigidBodyEnabled = false;
            onEndStandingUpAnimation?.Invoke();
        }
    }
}

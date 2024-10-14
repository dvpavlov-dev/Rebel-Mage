using System.Collections.Generic;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyView : MonoBehaviour
    {
        protected Animator AnimationController;
        protected List<Rigidbody> Rigidbodies;

        public void EnabledAnimator() => AnimationController.enabled = true;
        public void DisabledAnimator() => AnimationController.enabled = false;

        public void Init(Transform parent)
        {
            AnimationController = GetComponent<Animator>();
            Rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        }

        public virtual void ReactionOnExplosion(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
        }

        public void EnableRigidbody()
        {
            foreach (Rigidbody rb in Rigidbodies)
            {
                rb.isKinematic = false;
            }
        }

        public void DisableRigidbody()
        {
            foreach (Rigidbody rb in Rigidbodies)
            {
                rb.isKinematic = true;
            }
        }
    }
}

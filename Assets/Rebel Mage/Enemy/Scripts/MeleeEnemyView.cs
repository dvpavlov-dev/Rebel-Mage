using System;
using System.Linq;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyView : EnemyView
    {
        public event Action<string> OnEndAnimationAction;

        private Transform m_Parent;
        private Transform m_HipsBone;
        private bool IsFrontUp => Vector3.Dot(m_HipsBone.up, Vector3.up) > 0;
        
        private static readonly int MoveForward = Animator.StringToHash("MoveForward");
        private static readonly int MutantPunch = Animator.StringToHash("Mutant Punch");
        private const string ANIM_PUNCH_NAME = "Mutant Punch";
        private const string ANIM_STANDING_UP_FRONT_NAME = "Stand Up front";
        private const string ANIM_STANDING_UP_BACK_NAME = "Stand Up back";

        public override void Init(Transform parent)
        {
            base.Init(parent);
            
            m_Parent = parent;
            m_HipsBone = AnimationController.GetBoneTransform(HumanBodyBones.Hips);
        }

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
            // AnimationController.SetTrigger(MutantPunch);
            AnimationController.Play(ANIM_PUNCH_NAME);
        }

        public override void ReactionOnExplosion(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            Rigidbody hitBone = Rigidbodies.OrderBy(rigidbody => Vector3.Distance(positionImpact, rigidbody.position)).First();
            hitBone.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
        }

        public override void DisableRigidbody(Action onEndStandingUpAnimation)
        {
            base.DisableRigidbody(onEndStandingUpAnimation);
            
            StartAnimStandingUp(onEndStandingUpAnimation);
        }

        private void StartAnimStandingUp(Action onEndStandingUpAnimation)
        {
            AdjustParentPositionToHipsBone();
            AdjustParentRotationToHipsBone();

            OnEndAnimationAction += OnEndStandingUpAnimationAction;
            Debug.Log($"IsFrontUp: {IsFrontUp}");
            AnimationController.Play(IsFrontUp ? ANIM_STANDING_UP_FRONT_NAME : ANIM_STANDING_UP_BACK_NAME);

            void OnEndStandingUpAnimationAction(string animationName)
            {
                if(animationName is ANIM_STANDING_UP_FRONT_NAME or ANIM_STANDING_UP_BACK_NAME)
                {
                    OnEndAnimationAction -= OnEndStandingUpAnimationAction;
                    onEndStandingUpAnimation?.Invoke();
                }
            }
        }

        private void AdjustParentPositionToHipsBone()
        {
            Vector3 initHipsPosition = m_HipsBone.position;
            m_Parent.position = initHipsPosition;

            AdjustParentPositionRelativeGround();

            m_HipsBone.position = initHipsPosition;
        }

        private void AdjustParentPositionRelativeGround()
        {
            if (Physics.Raycast(m_Parent.position, Vector3.down, out RaycastHit hit, 5, 1 << LayerMask.NameToLayer("Walkable")))
            {
                m_Parent.position = new Vector3(m_Parent.position.x, hit.point.y + m_Parent.localScale.y / 2, m_Parent.position.z);
            }
        }

        private void AdjustParentRotationToHipsBone()
        {
            Vector3 initHipsPosition = m_HipsBone.position;
            Quaternion initHipsRotation = m_HipsBone.rotation;
            
            Vector3 directionForRotate = m_HipsBone.up;
            
            if(IsFrontUp == false)
            {
                directionForRotate *= -1;
            }

            directionForRotate.y = 0;
            
            Quaternion correctionForRotate = Quaternion.FromToRotation(m_Parent.forward, directionForRotate.normalized);
            m_Parent.rotation *= correctionForRotate;

            m_HipsBone.position = initHipsPosition;
            m_HipsBone.rotation = initHipsRotation;
        }
    }
}

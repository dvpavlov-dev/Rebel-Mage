using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyView : EnemyView
    {
        public List<Transform> ObjectNeedMoveWithModel = new();
        
        public event Action<string> OnEndAnimationAction;
        
        private const string ANIM_PUNCH_NAME = "Mutant Punch";
        private const string ANIM_SWIPING_NAME = "Mutant Swiping";
        private const string ANIM_STANDING_UP_FACE_UP_NAME = "Stand Up Face Up";
        private const string ANIM_STANDING_UP_FACE_DOWN_NAME = "Stand Up Face Down";

        private static readonly int m_MoveForward = Animator.StringToHash("MoveForward");

        private Transform m_HipsBone;
        private bool m_IsFacingUp;

        private Transform m_Parent;
        private RigAdjusterForAnimation m_RigAdjusterForDaceDownStandingUpAnimation;
        private RigAdjusterForAnimation m_RigAdjusterForFaceUpStandingUpAnimation;

        protected override void Update()
        {
            foreach (Transform obj in ObjectNeedMoveWithModel)
            {
                obj.position = new Vector3(m_HipsBone.position.x, obj.position.y, m_HipsBone.position.z);
            }
        }

        public override void Init(Transform parent)
        {
            base.Init(parent);

            m_Parent = parent;
            m_HipsBone = AnimationController.GetBoneTransform(HumanBodyBones.Hips);

            AnimationClip[] clips = AnimationController.runtimeAnimatorController.animationClips;
            Transform[] bones = AnimationController.GetComponentsInChildren<Transform>();

            m_RigAdjusterForFaceUpStandingUpAnimation = new RigAdjusterForAnimation(clips.First(clip => clip.name == ANIM_STANDING_UP_FACE_UP_NAME), bones, this);
            m_RigAdjusterForDaceDownStandingUpAnimation = new RigAdjusterForAnimation(clips.First(clip => clip.name == ANIM_STANDING_UP_FACE_DOWN_NAME), bones, this);
        }

        public void OnEndAnimation(string animationName) // Used in animation events
        {
            OnEndAnimationAction?.Invoke(animationName);
        }

        public void StartMoveAnimation(float moveCoefficient)
        {
            if(AnimationController.applyRootMotion)
            {
                AnimationController.applyRootMotion = false;
            }
            
            AnimationController.SetFloat(m_MoveForward, moveCoefficient);
        }

        public string StartPunchAnimation()
        {
            AnimationController.applyRootMotion = true;
            AnimationController.Play(ANIM_PUNCH_NAME);
            return ANIM_PUNCH_NAME;
        }

        public string StartSwipingAnimation()
        {
            AnimationController.applyRootMotion = true;
            AnimationController.Play(ANIM_SWIPING_NAME);
            return ANIM_SWIPING_NAME;
        }

        public override void ReactionOnExplosion(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            Rigidbody hitBone = Rigidbodies.OrderBy(rigidbody => Vector3.Distance(positionImpact, rigidbody.position)).First();
            hitBone.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
        }

        public override void DisableRigidbody(Action onEndStandingUpAnimation = null)
        {
            foreach (Rigidbody rb in Rigidbodies)
            {
                rb.isKinematic = true;
            }

            if (IsRigidBodyEnabled)
            {
                StartAnimStandingUp(onEndStandingUpAnimation);
            }

            IsRigidBodyEnabled = false;
        }

        private void StartAnimStandingUp(Action onEndStandingUpAnimation)
        {
            if(AnimationController.applyRootMotion)
            {
                AnimationController.applyRootMotion = false;
            }
            
            m_IsFacingUp = m_HipsBone.forward.y > 0;

            AdjustParentPositionToHipsBone();
            AdjustParentRotationToHipsBone();

            OnEndAnimationAction += OnEndStandingUpAnimationAction;

            if (m_IsFacingUp)
            {
                m_RigAdjusterForFaceUpStandingUpAnimation.Adjust(() => PlayStandingUpAnimation(ANIM_STANDING_UP_FACE_UP_NAME));
            }
            else
            {
                m_RigAdjusterForDaceDownStandingUpAnimation.Adjust(() => PlayStandingUpAnimation(ANIM_STANDING_UP_FACE_DOWN_NAME));
            }

            void OnEndStandingUpAnimationAction(string animationName)
            {
                if (animationName is ANIM_STANDING_UP_FACE_UP_NAME or ANIM_STANDING_UP_FACE_DOWN_NAME)
                {
                    OnEndAnimationAction -= OnEndStandingUpAnimationAction;
                    onEndStandingUpAnimation?.Invoke();
                }
            }
        }
        private void PlayStandingUpAnimation(string standingUpAnimationName)
        {
            EnabledAnimator();
            AnimationController.PlayInFixedTime(standingUpAnimationName, 0, 0);
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
                m_Parent.position = new Vector3(m_Parent.position.x, hit.point.y, m_Parent.position.z);
            }
        }

        private void AdjustParentRotationToHipsBone()
        {
            Vector3 initHipsPosition = m_HipsBone.position;
            Quaternion initHipsRotation = m_HipsBone.rotation;

            Vector3 directionForRotate = m_HipsBone.up;

            if (m_IsFacingUp)
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

    public class RigAdjusterForAnimation
    {
        private const float TIME_TO_SHIFT_BONES_TO_START_ANIMATION = 0.5f;

        private readonly List<Transform> m_Bones;
        private readonly BoneTransformData[] m_BonesAtStartAnimation;
        private readonly BoneTransformData[] m_BonesBeforeAnimation;

        private readonly AnimationClip m_Clip;
        private Coroutine m_ShiftBonesToStandingUpAnimation;
        private readonly MonoBehaviour m_View;

        public RigAdjusterForAnimation(AnimationClip clip, IEnumerable<Transform> bones, MonoBehaviour view)
        {
            m_Clip = clip;
            m_Bones = new List<Transform>(bones);
            m_View = view;

            m_BonesBeforeAnimation = new BoneTransformData[m_Bones.Count];
            m_BonesAtStartAnimation = new BoneTransformData[m_Bones.Count];

            for (int i = 0; i < m_Bones.Count; i++)
            {
                m_BonesBeforeAnimation[i] = new BoneTransformData();
                m_BonesAtStartAnimation[i] = new BoneTransformData();
            }

            SaveBonesDataFromStartAnimation();
        }

        private void SaveBonesDataFromStartAnimation()
        {
            Vector3 initPosition = m_View.transform.position;
            Quaternion initRotation = m_View.transform.rotation;

            m_Clip.SampleAnimation(m_View.gameObject, 0);
            SaveCurrentBoneDataTo(m_BonesAtStartAnimation);

            m_View.transform.position = initPosition;
            m_View.transform.rotation = initRotation;
        }

        private void SaveCurrentBoneDataTo(BoneTransformData[] bones)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].Position = m_Bones[i].localPosition;
                bones[i].Rotation = m_Bones[i].localRotation;
            }
        }

        public void Adjust(Action callback)
        {
            SaveCurrentBoneDataTo(m_BonesBeforeAnimation);

            if (m_ShiftBonesToStandingUpAnimation != null)
            {
                m_View.StopCoroutine(m_ShiftBonesToStandingUpAnimation);
            }

            m_ShiftBonesToStandingUpAnimation = m_View.StartCoroutine(ShiftBonesToAnimation(callback));
        }

        private IEnumerator ShiftBonesToAnimation(Action callback)
        {
            float progress = 0;

            while (progress < TIME_TO_SHIFT_BONES_TO_START_ANIMATION)
            {
                progress += Time.deltaTime;
                float progressInPercentage = progress / TIME_TO_SHIFT_BONES_TO_START_ANIMATION;

                for (int i = 0; i < m_Bones.Count; i++)
                {
                    m_Bones[i].localPosition = Vector3.Lerp(m_BonesBeforeAnimation[i].Position, m_BonesAtStartAnimation[i].Position, progressInPercentage);
                    m_Bones[i].localRotation = Quaternion.Lerp(m_BonesBeforeAnimation[i].Rotation, m_BonesAtStartAnimation[i].Rotation, progressInPercentage);
                }

                yield return null;
            }

            callback?.Invoke();
        }
    }

    public class BoneTransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}

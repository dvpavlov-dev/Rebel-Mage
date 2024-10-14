using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyAbilities : EnemyAbilities<MeleeEnemyView>
    {
        private bool m_IsAttackStarted;
        private const string ANIMATION_NAME = "Mutant Punch";

        private void FixedUpdate()
        {
            if (!IsEnemyAbilitiesSetup) return;
            
            if (Vector3.Distance(transform.position, Target.transform.position) < 2)
            {
                if (!m_IsAttackStarted)
                {
                    m_IsAttackStarted = true;
                 
                    EnemyController.EnemySM.ChangeState<AttackState<MeleeEnemyView>>();
                    EnemyView.StartAttackAnimation();
                    EnemyView.OnEndAnimationAction = OnEndAnimation;
                    // foreach (AnimationClip clip in EnemyController.AnimationController.runtimeAnimatorController.animationClips)
                    // {
                    //     if (clip.name == ANIMATION_NAME)
                    //     {
                    //         Invoke(nameof(OnEndAnimation), clip.length);
                    //     }
                    // }
                }
            }
        }

        private void OnEndAnimation(string animName)
        {
            OnEndPunchAnimation(animName);
        }
        
        private void OnEndPunchAnimation(string animName)
        {
            if (animName != "Mutant Punch") return;

            m_IsAttackStarted = false;

            EnemyController.EnemySM.ChangeState<MoveState<MeleeEnemyView>>();

            foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, 2, Vector3.up))
            {
                if (hit.transform.CompareTag("Player") && hit.transform.GetComponent<IDamage>() is {} damageController)
                {
                    damageController.TakeDamage(Damage);
                }
            }
        }
    }
}

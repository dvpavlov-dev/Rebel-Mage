using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyAbilities : EnemyAbilities
    {
        public Animator Animator;

        private bool m_IsAttackStarted;
        private const string ANIMATION_NAME = "Mutant Punch";

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, Target.transform.position) < 2)
            {
                if (!m_IsAttackStarted)
                {
                    m_IsAttackStarted = true;
                    Animator.SetTrigger(ANIMATION_NAME);
                    foreach (AnimationClip clip in Animator.runtimeAnimatorController.animationClips)
                    {
                        Debug.Log($"{clip.name}");
                        if (clip.name == ANIMATION_NAME)
                        {
                            Invoke(nameof(OnEndAnimation), clip.length);
                        }
                    }
                }
            }
        }

        private void OnEndAnimation()
        {
            m_IsAttackStarted = false;
            
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

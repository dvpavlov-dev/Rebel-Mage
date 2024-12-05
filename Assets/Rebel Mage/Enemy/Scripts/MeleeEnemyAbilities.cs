using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyAbilities : EnemyAbilities<MeleeEnemyView>
    {
        [SerializeField] private AudioClip attackSound;
        
        private string m_Animation_name;

        private void FixedUpdate()
        {
            if (!IsEnemyAbilitiesSetup) return;

            if (Vector3.Distance(transform.position, Target.transform.position) < 2)
            {
                if (!m_IsAttackStarted && CanAttack)
                {
                    m_IsAttackStarted = true;

                    AudioSource.clip = attackSound;
                    AudioSource.Play();
                    
                    EnemyController.SetAttackState();
                    m_Animation_name = EnemyView.StartPunchAnimation();

                    foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, 2, Vector3.up))
                    {
                        if (hit.transform.CompareTag("Player") && hit.transform.GetComponent<IDamage>() is {} damageController)
                        {
                            damageController.TakeDamage(Damage);
                        }
                    }

                    EnemyView.OnEndAnimationAction += OnEndAnimation;
                }
            }
        }

        private void OnEndAnimation(string animName)
        {
            EnemyView.OnEndAnimationAction -= OnEndAnimation;
            OnEndPunchAnimation(animName);
        }

        private void OnEndPunchAnimation(string animName)
        {
            if (animName != m_Animation_name) return;

            m_IsAttackStarted = false;

            EnemyController.SetMoveState();
        }
    }
}

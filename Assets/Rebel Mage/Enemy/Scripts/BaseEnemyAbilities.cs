using System.Collections;
using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(AudioSource))]
    public class BaseEnemyAbilities : EnemyAbilities<BaseEnemyView>
    {
        [SerializeField] private AudioClip attackSound;

        private void OnTriggerEnter(Collider other)
        {
            if (CanAttack && !m_IsAttackStarted && other.CompareTag("Player") && other.GetComponent<IDamage>() is {} damageController)
            {
                m_IsAttackStarted = true;
                EnemyController.SetAttackState();
                
                EnemyView.ActivateAttackEffect();
                damageController.TakeDamage(Damage);

                AudioSource.clip = attackSound;
                AudioSource.Play();
                
                StartCoroutine(OnEndAttack());
            }
        }
        
        private IEnumerator OnEndAttack()
        {
            yield return new WaitForSeconds(1);
            
            m_IsAttackStarted = false;
            EnemyController.SetMoveState();
        }
    }
}

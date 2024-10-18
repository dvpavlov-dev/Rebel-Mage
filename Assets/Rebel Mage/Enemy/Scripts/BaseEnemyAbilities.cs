using System.Collections;
using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class BaseEnemyAbilities : EnemyAbilities<BaseEnemyView>
    {
        private void OnTriggerEnter(Collider other)
        {
            if (CanAttack && !m_IsAttackStarted && other.CompareTag("Player") && other.GetComponent<IDamage>() is {} damageController)
            {
                m_IsAttackStarted = true;
                EnemyController.SetAttackState();
                
                EnemyView.ActivateAttackEffect();
                damageController.TakeDamage(Damage);

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

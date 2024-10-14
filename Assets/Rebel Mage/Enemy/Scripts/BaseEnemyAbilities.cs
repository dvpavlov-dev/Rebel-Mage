using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class BaseEnemyAbilities : EnemyAbilities<EnemyView>
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.GetComponent<IDamage>() is {} damageController)
            {
                damageController.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
        }
    }
}

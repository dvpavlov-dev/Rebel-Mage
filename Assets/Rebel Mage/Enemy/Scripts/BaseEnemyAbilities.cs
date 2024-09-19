using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Vanguard_Drone.Enemy
{
    public class BaseEnemyAbilities : EnemyAbilities
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IDamage>() is {} damageController && other.CompareTag("Player"))
            {
                damageController.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections;
using PushItOut.Player;
using UnityEngine;

namespace PushItOut.Spell_system
{
    public class DamageController : MonoBehaviour, IDamage
    {
        public float MaxHealth = 1;
        public HealthUI HealthUI;

        private float Health {
            get => _health;
            set 
            {
                _health = value;
                SyncHealth();
            }
        }

        private float _health;

        private void Start()
        {
            InitHealthPoints();
        }

        public void InitHealthPoints(float maxHealth)
        {
            MaxHealth = maxHealth;
            InitHealthPoints();
        }
        
        private void InitHealthPoints()
        {
            Health = MaxHealth;

            if (HealthUI != null)
            {
                HealthUI.Constructor(MaxHealth);
                HealthUI.UpdateHp(Health);
            }
        }

        private void SyncHealth()
        {
            if (HealthUI != null)
            {
                HealthUI.UpdateHp(Health);
            }
            
            if (Health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    
        public void TakeDamage(float damage)
        {
            if (this == null && gameObject.activeSelf) return;
            
            Health -= damage;
        }
        
        public void TakePeriodDamage(float damage, float interval, float time)
        {
            StartCoroutine( ActivatePeriodDamage(damage,interval,time));
        }
        
        private IEnumerator ActivatePeriodDamage(float damage, float interval, float time)
        {
            float currentTime = 0;
            while (currentTime < time)
            {
                TakeDamage(damage);
                currentTime += interval;
                yield return new WaitForSeconds(interval);
            }
        }

        private void OnDisable()
        {
            InitHealthPoints();
        }
    }
}
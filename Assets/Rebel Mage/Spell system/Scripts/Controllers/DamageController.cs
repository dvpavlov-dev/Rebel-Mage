using System;
using System.Collections;
using Rebel_Mage.Player;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    [RequireComponent(typeof(BoxCollider))]
    public class DamageController : MonoBehaviour, IDamage
    {
        public HealthUI HealthUI;
        public Action OnDead { get; set; }

        private float Health {
            get => m_Health;
            set 
            {
                m_Health = value;
                SyncHealth();
            }
        }

        private float m_MaxHealth = 1;
        private float m_Health;
        
        public void InitHealthPoints(float maxHealth)
        {
            m_MaxHealth = maxHealth;
            InitHealthPoints();
        }
        
        private void InitHealthPoints()
        {
            Health = m_MaxHealth;

            if (HealthUI != null)
            {
                HealthUI.Constructor(m_MaxHealth);
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
                // gameObject.SetActive(false);
                HealthUI.gameObject.SetActive(false);
                OnDead?.Invoke();
            }
        }
    
        public void TakeDamage(float damage)
        {
            if (this == null || Health <= 0) return;
            
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
    }
}
using System;
using Rebel_Mage.UI;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    [RequireComponent(typeof(BoxCollider))]
    public class DamageController : MonoBehaviour, IDamage
    {
        public HealthUI HealthUI;
        public Action OnDead { get; set; }

        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private float Health {
            get => _health;
            set 
            {
                _health = value;
                SyncHealth();
            }
        }

        private float _maxHealth = 1;
        private float _health;
        
        public void InitHealthPoints(float maxHealth)
        {
            HealthUI.gameObject.SetActive(true);
            _collider.enabled = true;
            
            _maxHealth = maxHealth;
            InitHealthPoints();
        }
        
        private void InitHealthPoints()
        {
            Health = _maxHealth;

            if (HealthUI != null)
            {
                HealthUI.Constructor(_maxHealth);
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
                HealthUI.gameObject.SetActive(false);

                _collider.enabled = false;
                OnDead?.Invoke();
            }
        }
    
        public void TakeDamage(float damage)
        {
            if (this == null || Health <= 0) return;
            
            Health -= damage;
        }
    }
}
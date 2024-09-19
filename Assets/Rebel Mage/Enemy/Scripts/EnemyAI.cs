using System;
using System.Collections;
using Rebel_Mage.Spell_system;
using UnityEngine;
using UnityEngine.AI;

namespace Vanguard_Drone.Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
    public class EnemyAI : MonoBehaviour, IImpact
    {
        protected GameObject Target;
        protected NavMeshAgent Agent;
        protected bool IsEnemySetup;
    
        private bool _isBlockedControl;
        private Rigidbody _rb;
        private float _moveSpeed;
        private Coroutine _timerForSpeedEffects;

        public virtual void SetupEnemyAI(float moveSpeed, GameObject target)
        {
            Target = target;
            _moveSpeed = moveSpeed;
        
            Agent = GetComponent<NavMeshAgent>();
            Agent.speed = _moveSpeed;

            _rb = GetComponent<Rigidbody>();

            IsEnemySetup = true;
        }
    
        public void ExplosionImpact(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            _rb.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
        }
    
        public void ChangeSpeedImpact(float slowdownPercentage, float timeSlowdown)
        {
            if (!gameObject.activeSelf) return;
            
            Agent.speed = _moveSpeed - _moveSpeed * slowdownPercentage;
            
            if (_timerForSpeedEffects != null)
            {
                StopCoroutine(_timerForSpeedEffects);
            }
            
            _timerForSpeedEffects = StartCoroutine(ReturnSpeed(timeSlowdown));
        }
    
        private IEnumerator ReturnSpeed(float timeWhenReturn)
        {
            yield return new WaitForSeconds(timeWhenReturn);
            Agent.speed = _moveSpeed;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            
            StopAllCoroutines();
        }
    }
}

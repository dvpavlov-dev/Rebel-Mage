using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rebel_Mage.Spell_system;
using UnityEngine;
using UnityEngine.AI;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI : MonoBehaviour, IImpact
    {
        protected GameObject Target;
        protected NavMeshAgent Agent;
        protected bool IsEnemySetup;
        protected float MoveCoefficient = 1;

        private Enemy m_Enemy;
        private bool m_IsBlockControl;
        // private Rigidbody m_Rb;
        private List<Rigidbody> m_Rigidbodies;
        private float m_MoveSpeed;
        private Coroutine m_TimerForSpeedEffects;

        protected virtual void FixedUpdate()
        {
            if (!IsEnemySetup || IsBlockControl) return;
            
            Agent.SetDestination(Target.transform.position);
        }
        
        protected bool IsBlockControl 
        {
            get => m_IsBlockControl;
            private set 
            {
                m_IsBlockControl = value;
                Agent.enabled = !value;
            }
        }
        
        public void SetupEnemyAI(float moveSpeed, GameObject target, float stoppingDistance, Enemy enemy)
        {
            Target = target;
            m_MoveSpeed = moveSpeed;
            m_Enemy = enemy;
        
            Agent = GetComponent<NavMeshAgent>();
            Agent.speed = m_MoveSpeed;
            Agent.stoppingDistance = stoppingDistance;

            // m_Rb = GetComponent<Rigidbody>();
            m_Rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
            DisableRigidbody();

            IsEnemySetup = true;
        }

        void IImpact.ExplosionImpact(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            IsBlockControl = true;
            // m_Rb.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
            Hit(positionImpact, maxDistance, explosionForce);
            // Invoke(nameof(SetBlockControl), 1f);
        }

        private void Hit(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            IsBlockControl = true;
            EnableRigidbody();

            Rigidbody hitBone = m_Rigidbodies.OrderBy(rigidbody => Vector3.Distance(positionImpact, rigidbody.position)).First();
            hitBone.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
        }
        
        private void EnableRigidbody()
        {
            foreach (Rigidbody rb in m_Rigidbodies)
            {
                rb.isKinematic = false;
            }
        }

        private void DisableRigidbody()
        {
            foreach (Rigidbody rb in m_Rigidbodies)
            {
                rb.isKinematic = true;
            }
        }

        private void SetBlockControl()
        {
            Agent.speed = 0;
            IsBlockControl = false;
            Agent.speed = m_MoveSpeed * MoveCoefficient;
        }

        void IImpact.ChangeSpeedImpact(float slowdownPercentage, float timeSlowdown)
        {
            if (!gameObject.activeSelf) return;
            
            MoveCoefficient = 1 - slowdownPercentage;
            Agent.speed = m_MoveSpeed * MoveCoefficient;
            
            if (m_TimerForSpeedEffects != null)
            {
                StopCoroutine(m_TimerForSpeedEffects);
            }
            
            m_TimerForSpeedEffects = StartCoroutine(ReturnSpeed(timeSlowdown));
        }
    
        private IEnumerator ReturnSpeed(float timeWhenReturn)
        {
            yield return new WaitForSeconds(timeWhenReturn);
            Agent.speed = m_MoveSpeed;
            MoveCoefficient = 1;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            
            StopAllCoroutines();
        }
    }
}

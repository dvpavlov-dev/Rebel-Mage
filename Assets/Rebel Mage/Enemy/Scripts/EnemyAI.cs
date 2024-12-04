using System.Collections;
using Rebel_Mage.Spell_system;
using UnityEngine;
using UnityEngine.AI;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI<T> : MonoBehaviour, IImpact where T : EnemyView
    {
        public bool AgentEnabled {
            get => m_Agent.enabled;
            set 
            {
                if (value)
                {
                    m_Agent.speed = 0;
                    m_Agent.enabled = true;
                    m_Agent.speed = m_MoveSpeed * MoveCoefficient;
                }
                else
                {
                    m_Agent.speed = m_MoveSpeed * MoveCoefficient;
                    m_Agent.enabled = false;
                    m_Agent.speed = 0;
                }
            }
        }
        
        protected bool IsEnemySetup;
        protected GameObject Target;
        protected float MoveCoefficient = 1;
        protected Enemy<T> EnemyController;
        protected T EnemyView;

        protected NavMeshAgent m_Agent;
        private float m_MoveSpeed;
        private Coroutine m_TimerForSpeedEffects;
        
        public void SetupEnemyAI(float moveSpeed, GameObject target, float stoppingDistance, T enemyView, Enemy<T> enemy)
        {
            Target = target;
            m_MoveSpeed = moveSpeed;
            EnemyView = enemyView;
            EnemyController = enemy;

            m_Agent = GetComponent<NavMeshAgent>();
            m_Agent.speed = m_MoveSpeed;
            m_Agent.stoppingDistance = stoppingDistance;
            
            IsEnemySetup = true;
        }

        protected virtual void FixedUpdate()
        {
            if (!IsEnemySetup || !m_Agent.enabled) return;

            m_Agent.SetDestination(Target.transform.position);
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;

            StopAllCoroutines();
        }

        void IImpact.ExplosionImpact(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            EnemyController.SetRagdollActivatedState();
            Hit(positionImpact, maxDistance, explosionForce);
        }

        void IImpact.ChangeSpeedImpact(float slowdownPercentage, float timeSlowdown)
        {
            if (!gameObject.activeSelf) return;

            MoveCoefficient = 1 - slowdownPercentage;
            m_Agent.speed = m_MoveSpeed * MoveCoefficient;

            if (m_TimerForSpeedEffects != null)
            {
                StopCoroutine(m_TimerForSpeedEffects);
            }

            m_TimerForSpeedEffects = StartCoroutine(ReturnSpeed(timeSlowdown));
        }
        
        private void Hit(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            // EnemyController.EnemyView.EnableRigidbody();
            EnemyController.EnemyView.ReactionOnExplosion(positionImpact,maxDistance,explosionForce);
            
            StartCoroutine(ReturnControl());
        }

        private IEnumerator ReturnControl()
        {
            yield return new WaitForSeconds(2f);
            
            EnemyController.SetMoveState();
        }

        private IEnumerator ReturnSpeed(float timeWhenReturn)
        {
            yield return new WaitForSeconds(timeWhenReturn);
            m_Agent.speed = m_MoveSpeed;
            MoveCoefficient = 1;
        }
    }
}

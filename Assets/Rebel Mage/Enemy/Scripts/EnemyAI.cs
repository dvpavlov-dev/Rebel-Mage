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

        // public bool RagdollEnabled 
        // {
        //     set 
        //     {
        //         if (value)
        //         {
        //             EnableRigidbody();
        //         }
        //         else
        //         {
        //             DisableRigidbody();
        //         }
        //     }
        // }
        
        protected bool IsEnemySetup;
        protected GameObject Target;
        protected float MoveCoefficient = 1;
        protected Enemy EnemyController;

        private NavMeshAgent m_Agent;
        private float m_MoveSpeed;
        private List<Rigidbody> m_Rigidbodies;
        private Coroutine m_TimerForSpeedEffects;
        
        public void SetupEnemyAI(float moveSpeed, GameObject target, float stoppingDistance, Enemy enemy)
        {
            Target = target;
            m_MoveSpeed = moveSpeed;
            EnemyController = enemy;

            m_Agent = GetComponent<NavMeshAgent>();
            m_Agent.speed = m_MoveSpeed;
            m_Agent.stoppingDistance = stoppingDistance;

            m_Rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
            EnemyController.MeleeEnemyView.DisableRigidbody();

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
            EnemyController.EnemySM.ChangeState<KnockedDownState>();
            // m_Rb.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
            Hit(positionImpact, maxDistance, explosionForce);
            // Invoke(nameof(SetBlockControl), 1f);
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
            EnemyController.MeleeEnemyView.EnableRigidbody();
            EnemyController.MeleeEnemyView.ReactionOnExplosion(positionImpact,maxDistance,explosionForce);
            // Rigidbody hitBone = m_Rigidbodies.OrderBy(rigidbody => Vector3.Distance(positionImpact, rigidbody.position)).First();
            // hitBone.AddExplosionForce(explosionForce, positionImpact, maxDistance, 0, ForceMode.Impulse);
            
            Invoke(nameof(ReturnControl), 2f);
        }

        private void ReturnControl()
        {
            EnemyController.MeleeEnemyView.DisableRigidbody();
            EnemyController.EnemySM.ChangeState<MoveState>();
        }

        // private void EnableRigidbody()
        // {
        //     foreach (Rigidbody rb in m_Rigidbodies)
        //     {
        //         rb.isKinematic = false;
        //     }
        // }
        //
        // private void DisableRigidbody()
        // {
        //     foreach (Rigidbody rb in m_Rigidbodies)
        //     {
        //         rb.isKinematic = true;
        //     }
        // }

        private IEnumerator ReturnSpeed(float timeWhenReturn)
        {
            yield return new WaitForSeconds(timeWhenReturn);
            m_Agent.speed = m_MoveSpeed;
            MoveCoefficient = 1;
        }
    }
}

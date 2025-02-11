using System.Collections;
using Rebel_Mage.Spell_system;
using UnityEngine;
using UnityEngine.AI;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI<T> : MonoBehaviour, IImpact where T : EnemyView
    {
        [SerializeField] private Rigidbody _rb;
        
        public bool AgentEnabled {
            get => _agent.enabled;
            set 
            {
                if (value)
                {
                    _rb.isKinematic = true;
                    _rb.useGravity = false;
                    
                    _agent.speed = 0;
                    _agent.enabled = true;
                    _agent.speed = _moveSpeed * MoveCoefficient;
                }
                else
                {
                    // _agent.speed = _moveSpeed * MoveCoefficient;
                    _agent.speed = 0;
                    _agent.enabled = false;
                    
                    _rb.isKinematic = false;
                    _rb.useGravity = true;
                    _rb.velocity = Vector3.zero;
                }
            }
        }
        
        protected bool IsEnemySetup;
        protected float MoveCoefficient = 1;
        protected T EnemyView;
        protected NavMeshAgent _agent;

        private GameObject _target;
        private Enemy<T> _enemyController;
        private float _moveSpeed;
        private Coroutine _timerForSpeedEffects;
        
        public void SetupEnemyAI(float moveSpeed, GameObject target, float stoppingDistance, T enemyView, Enemy<T> enemy)
        {
            _target = target;
            _moveSpeed = moveSpeed;
            EnemyView = enemyView;
            _enemyController = enemy;

            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = _moveSpeed;
            _agent.stoppingDistance = stoppingDistance;
            
            IsEnemySetup = true;
        }

        protected virtual void FixedUpdate()
        {
            if (!IsEnemySetup || !_agent.enabled) return;

            _agent.SetDestination(_target.transform.position);
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;

            StopAllCoroutines();
        }

        void IImpact.ExplosionImpact(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            _enemyController.SetRagdollActivatedState();
            Hit(positionImpact, maxDistance, explosionForce);
        }

        void IImpact.ChangeSpeedImpact(float slowdownPercentage, float timeSlowdown)
        {
            if (!gameObject.activeSelf) return;

            MoveCoefficient = 1 - slowdownPercentage;
            _agent.speed = _moveSpeed * MoveCoefficient;

            if (_timerForSpeedEffects != null)
            {
                StopCoroutine(_timerForSpeedEffects);
            }

            _timerForSpeedEffects = StartCoroutine(ReturnSpeed(timeSlowdown));
        }
        
        private void Hit(Vector3 positionImpact, float maxDistance, float explosionForce)
        {
            _enemyController.EnemyView.ReactionOnExplosion(positionImpact,maxDistance,explosionForce);
            
            if (!gameObject.activeSelf) return;
            
            StartCoroutine(ReturnControl());
        }

        private IEnumerator ReturnControl()
        {
            yield return new WaitForSeconds(0.2f);
            
            _enemyController.SetMoveState();
        }

        private IEnumerator ReturnSpeed(float timeWhenReturn)
        {
            yield return new WaitForSeconds(timeWhenReturn);
            _agent.speed = _moveSpeed;
            MoveCoefficient = 1;
        }
    }
}

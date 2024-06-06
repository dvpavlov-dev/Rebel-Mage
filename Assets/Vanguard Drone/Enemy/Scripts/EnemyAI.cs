using PushItOut.Spell_system;
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
        private float _currentSpeed;

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
            Agent.speed = _moveSpeed - _moveSpeed * slowdownPercentage;
            Invoke(nameof(ReturnSpeed), timeSlowdown);
        }
    
        private void ReturnSpeed()
        {
            Agent.speed = _moveSpeed;
        }
    }
}

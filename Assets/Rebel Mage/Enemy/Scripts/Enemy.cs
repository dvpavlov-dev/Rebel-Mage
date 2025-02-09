using System;
using System.Collections.Generic;
using Rebel_Mage.Configs;
using Rebel_Mage.Infrastructure;
using Rebel_Mage.Spell_system;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(DamageController), typeof(AudioSource))]
    public class Enemy<T> : MonoBehaviour where T : EnemyView
    {
        [SerializeField] private AudioClip deathSound;
        
        public T EnemyView;
        
        public int PointsForEnemy { get; protected set; }
        public bool IsEnemyDead { get; private set; }

        protected GeneralEnemyParameters _Config;
        protected IDamage _DmgController { get; private set; }
        protected EnemyAbilities<T> _EnemyAbilities { get; set; }
        protected EnemyAI<T> _EnemyAI { get; set; }
        protected AudioSource _AudioSource { get; set; }

        private EnemyStateMachine<T> _enemySm;
        private IActorsFactory _actorsFactory;

        [Inject]
        private void Constructor(IActorsFactory actorsFactory)
        {
            // _factoryActors = factoryActors;
        }
        
        private void Awake()
        {
            _AudioSource = GetComponent<AudioSource>();
            _AudioSource.playOnAwake = false;
        }

        public virtual void InitEnemy(Infrastructure.Configs configs, GameObject target, Action onDead, ActorsFactory actorsFactory)
        {
            _actorsFactory = actorsFactory;
            EnemyView.Init(transform);
            _enemySm = new EnemyStateMachine<T>(this, _EnemyAI, _EnemyAbilities, EnemyView);

            _DmgController = GetComponent<IDamage>();
            _DmgController.OnDead = () => 
            {
                OnDeadAction(onDead);
            };
        }
        
        public void SetMoveState()
        {
            _enemySm.ChangeState<MoveState<T>>();
        }

        public void SetAttackState()
        {
            _enemySm.ChangeState<AttackState<T>>();
        }

        public void SetRagdollActivatedState()
        {
            _enemySm.ChangeState<RagdollActivatedState<T>>();
        }

        private void SetDeadState()
        {
            _enemySm.ChangeState<DeadState<T>>();
        }
        
        private void OnDeadAction(Action onDead)
        {
            SetDeadState();
            IsEnemyDead = true;

            _AudioSource.clip = deathSound;
            _AudioSource.Play();
            
            onDead?.Invoke();
            
            // _factoryActors.DisposeEnemy(_Config.EnemyType, gameObject);
        }
        
        private void OnDisable()
        {
            if(gameObject.activeSelf)
            {
                _actorsFactory.DisposeEnemy(_Config.EnemyType, gameObject);
            }
        }
    }

    public class EnemyStateMachine<T> where T : EnemyView
    {
        private readonly Enemy<T> _enemy;
        private readonly Dictionary<Type, IStateEnemy> _states;

        private IStateEnemy _activeState;

        public EnemyStateMachine(Enemy<T> enemy, EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T meleeEnemyView)
        {
            _enemy = enemy;
            _states = new Dictionary<Type, IStateEnemy>
            {
                [typeof(MoveState<T>)] = new MoveState<T>(enemyAI, enemyAbilities, meleeEnemyView),
                [typeof(AttackState<T>)] = new AttackState<T>(enemyAI, enemyAbilities),
                [typeof(RagdollActivatedState<T>)] = new RagdollActivatedState<T>(enemyAI, enemyAbilities, meleeEnemyView),
                [typeof(DeadState<T>)] = new DeadState<T>(enemyAI, enemyAbilities, meleeEnemyView),
            };
        }

        public void ChangeState<TState>() where TState : class, IStateEnemy
        {
            if (_enemy.IsEnemyDead) return;
            
            _activeState?.Exit();

            _activeState = GetState<TState>();
            _activeState.Enter();
        }

        private TState GetState<TState>() where TState : class, IStateEnemy => _states[typeof(TState)] as TState;
    }

    class MoveState<T> : IStateEnemy where T : EnemyView
    {
        private readonly EnemyAbilities<T> _enemyAbilities;
        private readonly EnemyAI<T> _enemyAI;
        private readonly T _enemyView;

        public MoveState(EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T enemyView)
        {
            _enemyAI = enemyAI;
            _enemyAbilities = enemyAbilities;
            _enemyView = enemyView;
        }

        public void Enter()
        {
            _enemyAbilities.CanAttack = true;
            
            if(_enemyView.IsRigidBodyEnabled)
            {
                _enemyView.DisableRigidbody(() => 
                {
                    _enemyAI.AgentEnabled = true;
                });
            }
            else
            {
                _enemyAI.AgentEnabled = true;
            }
        }

        public void Exit() {}
    }

    class AttackState<T> : IStateEnemy where T : EnemyView
    {
        private readonly EnemyAbilities<T> _enemyAbilities;
        private readonly EnemyAI<T> _enemyAI;

        public AttackState(EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities)
        {
            _enemyAI = enemyAI;
            _enemyAbilities = enemyAbilities;
        }

        public void Enter()
        {
            _enemyAI.AgentEnabled = false;
            _enemyAbilities.CanAttack = false;
        }

        public void Exit()
        {
            _enemyAbilities.IsAttackInProgress = false;
            _enemyAbilities.StopAllCoroutines();
        }
    }

    class RagdollActivatedState<T> : IStateEnemy where T : EnemyView
    {
        private readonly EnemyAbilities<T> _enemyAbilities;
        private readonly EnemyAI<T> _enemyAI;
        private readonly T _enemyView;

        public RagdollActivatedState(EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T enemyView)
        {
            _enemyAI = enemyAI;
            _enemyAbilities = enemyAbilities;
            _enemyView = enemyView;
        }
        public void Enter()
        {
            _enemyAI.AgentEnabled = false;
            _enemyAbilities.CanAttack = false;
            
            // _enemyView.EnableRigidbody();
        }

        public void Exit() {}
    }

    class DeadState<T> : IStateEnemy where T : EnemyView
    {
        private readonly EnemyAI<T> _enemyAI;
        private readonly EnemyAbilities<T> _enemyAbilities;
        private readonly EnemyView _enemyView;
        
        public DeadState(EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, EnemyView enemyView)
        {
            _enemyAI = enemyAI;
            _enemyAbilities = enemyAbilities;
            _enemyView = enemyView;
        }
        
        public void Enter()
        {
            _enemyAI.AgentEnabled = false;
            _enemyAbilities.CanAttack = false;
            _enemyView.EnableRigidbody();
        }
        
        public void Exit() {}
    }

    public interface IStateEnemy
    {
        void Enter();
        void Exit();
    }
}

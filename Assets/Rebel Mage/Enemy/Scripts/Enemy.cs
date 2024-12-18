using System;
using System.Collections.Generic;
using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(DamageController), typeof(AudioSource))]
    public class Enemy<T> : MonoBehaviour where T : EnemyView
    {
        [SerializeField] private AudioClip deathSound;
        
        public T EnemyView;
        public int PointsForEnemy { get; protected set; }
        public bool IsEnemyDead { get; private set; }
        public IDamage DmgController { get; private set; }
        
        protected EnemyAbilities<T> EnemyAbilities { get; set; }
        protected EnemyAI<T> EnemyAI { get; set; }
        protected AudioSource audioSource { get; set; }

        private EnemyStateMachine<T> m_EnemySm;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        public virtual void InitEnemy(Configs.Configs configs, GameObject target, Action onDead)
        {
            EnemyView.Init(transform);
            m_EnemySm = new EnemyStateMachine<T>(this, EnemyAI, EnemyAbilities, EnemyView);

            DmgController = GetComponent<IDamage>();
            DmgController.OnDead = () => 
            {
                OnDeadAction(onDead);
            };
        }

        protected virtual void OnDeadAction(Action onDead)
        {
            SetDeadState();
            IsEnemyDead = true;

            audioSource.clip = deathSound;
            audioSource.Play();
            
            onDead?.Invoke();
        }

        public void SetMoveState()
        {
            m_EnemySm.ChangeState<MoveState<T>>();
        }

        public void SetAttackState()
        {
            m_EnemySm.ChangeState<AttackState<T>>();
        }

        public void SetRagdollActivatedState()
        {
            m_EnemySm.ChangeState<RagdollActivatedState<T>>();
        }

        private void SetDeadState()
        {
            m_EnemySm.ChangeState<DeadState<T>>();
        }
    }

    public class EnemyStateMachine<T> where T : EnemyView
    {
        private readonly Enemy<T> m_Enemy;
        private readonly Dictionary<Type, IStateEnemy> m_States;

        private IStateEnemy m_ActiveState;

        public EnemyStateMachine(Enemy<T> enemy, EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T meleeEnemyView)
        {
            m_Enemy = enemy;
            m_States = new Dictionary<Type, IStateEnemy>
            {
                [typeof(MoveState<T>)] = new MoveState<T>(enemyAI, enemyAbilities, meleeEnemyView),
                [typeof(AttackState<T>)] = new AttackState<T>(enemyAI, enemyAbilities),
                [typeof(RagdollActivatedState<T>)] = new RagdollActivatedState<T>(enemyAI, enemyAbilities, meleeEnemyView),
                [typeof(DeadState<T>)] = new DeadState<T>(enemyAI, enemyAbilities, meleeEnemyView),
            };
        }

        public void ChangeState<TState>() where TState : class, IStateEnemy
        {
            if (m_Enemy.IsEnemyDead) return;
            
            m_ActiveState?.Exit();

            m_ActiveState = GetState<TState>();
            m_ActiveState.Enter();
        }

        private TState GetState<TState>() where TState : class, IStateEnemy => m_States[typeof(TState)] as TState;
    }

    class MoveState<T> : IStateEnemy where T : EnemyView
    {
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly EnemyAI<T> m_EnemyAI;
        private readonly T m_EnemyView;

        public MoveState(EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T enemyView)
        {
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
            m_EnemyView = enemyView;
        }

        public void Enter()
        {
            m_EnemyAbilities.CanAttack = true;
            
            if(m_EnemyView.IsRigidBodyEnabled)
            {
                m_EnemyView.DisableRigidbody(() => 
                {
                    m_EnemyAI.AgentEnabled = true;
                });
            }
            else
            {
                m_EnemyAI.AgentEnabled = true;
            }
        }

        public void Exit() {}
    }

    class AttackState<T> : IStateEnemy where T : EnemyView
    {
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly EnemyAI<T> m_EnemyAI;

        public AttackState(EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities)
        {
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
        }

        public void Enter()
        {
            m_EnemyAI.AgentEnabled = false;
            m_EnemyAbilities.CanAttack = false;
        }

        public void Exit()
        {
            m_EnemyAbilities.IsAttackInProgress = false;
            m_EnemyAbilities.StopAllCoroutines();
        }
    }

    class RagdollActivatedState<T> : IStateEnemy where T : EnemyView
    {
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly EnemyAI<T> m_EnemyAI;
        private readonly T m_EnemyView;

        public RagdollActivatedState(EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T enemyView)
        {
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
            m_EnemyView = enemyView;
        }
        public void Enter()
        {
            m_EnemyAI.AgentEnabled = false;
            m_EnemyAbilities.CanAttack = false;
            
            m_EnemyView.EnableRigidbody();
        }

        public void Exit() {}
    }

    class DeadState<T> : IStateEnemy where T : EnemyView
    {
        private readonly EnemyAI<T> m_EnemyAI;
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly EnemyView m_EnemyView;
        
        public DeadState(EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, EnemyView enemyView)
        {
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
            m_EnemyView = enemyView;
        }
        
        public void Enter()
        {
            m_EnemyAI.AgentEnabled = false;
            m_EnemyAbilities.CanAttack = false;
            m_EnemyView.EnableRigidbody();
        }
        
        public void Exit() {}
    }

    public interface IStateEnemy
    {
        void Enter();
        void Exit();
    }
}

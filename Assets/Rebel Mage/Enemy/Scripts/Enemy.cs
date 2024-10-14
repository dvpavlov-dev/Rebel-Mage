using System;
using System.Collections.Generic;
using Rebel_Mage.Spell_system;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(DamageController))]
    public class Enemy<T> : MonoBehaviour where T : EnemyView
    {
        [FormerlySerializedAs("MeleeEnemyView")]
        public T EnemyView;
        public EnemyStateMachine<T> EnemySM;

        public int PointsForEnemy { get; protected set; }

        protected DamageController DmgController;
        protected EnemyAbilities<T> EnemyAbilities;
        protected EnemyAI<T> EnemyAI;

        public virtual void InitEnemy(Configs.Configs configs, GameObject target, Action onDead)
        {
            EnemyView.Init(transform);
            EnemySM = new EnemyStateMachine<T>(this, EnemyAI, EnemyAbilities, EnemyView);
            
            DmgController = GetComponent<DamageController>();
            DmgController.OnDead = () => 
            {
                onDead?.Invoke();
                gameObject.SetActive(false);
            };
        }
    }

    public class EnemyStateMachine<T> where T : EnemyView
    {
        private readonly Dictionary<Type, IStateEnemy> m_States;
        
        private IStateEnemy m_ActiveState;
        
        public EnemyStateMachine(Enemy<T> enemy, EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T meleeEnemyView)
        {
            m_States = new Dictionary<Type, IStateEnemy>
            {
                [typeof(MoveState<T>)] = new MoveState<T>(enemy, enemyAI, enemyAbilities, meleeEnemyView),
                [typeof(AttackState<T>)] = new AttackState<T>(enemy, enemyAI, enemyAbilities, meleeEnemyView),
                [typeof(KnockedDownState<T>)] = new KnockedDownState<T>(enemy, enemyAI, enemyAbilities, meleeEnemyView),
            };
        }

        public void ChangeState<TState>() where TState : class, IStateEnemy
        {
            m_ActiveState?.Exit();

            m_ActiveState = GetState<TState>();
            m_ActiveState.Enter();
        }
        
        private TState GetState<TState>() where TState : class, IStateEnemy => m_States[typeof(TState)] as TState;
    }

    class MoveState<T> : IStateEnemy where T : EnemyView
    {
        private readonly Enemy<T> m_Enemy;
        private readonly EnemyAI<T> m_EnemyAI;
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly T m_MeleeEnemyView;

        public MoveState(Enemy<T> enemy, EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T meleeEnemyView)
        {
            m_Enemy = enemy;
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
            m_MeleeEnemyView = meleeEnemyView;
        }
        
        public void Enter()
        {
            m_EnemyAI.AgentEnabled = true;
            m_MeleeEnemyView.DisableRigidbody();
            
            m_MeleeEnemyView.EnabledAnimator();
        }
        
        public void Exit()
        {
            
        }
    } 
    
    class AttackState<T> : IStateEnemy where T : EnemyView
    {
        private readonly Enemy<T> m_Enemy;
        private readonly EnemyAI<T> m_EnemyAI;
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly T m_MeleeEnemyView;

        public AttackState(Enemy<T> enemy, EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T meleeEnemyView)
        {
            m_Enemy = enemy;
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
            m_MeleeEnemyView = meleeEnemyView;
        }
        
        public void Enter()
        {
            m_EnemyAI.AgentEnabled = false;
            m_MeleeEnemyView.DisableRigidbody();

            m_MeleeEnemyView.EnabledAnimator();
        }
        
        public void Exit()
        {
            
        }
    } 
    
    class KnockedDownState<T> : IStateEnemy where T : EnemyView
    {
        private readonly Enemy<T> m_Enemy;
        private readonly EnemyAI<T> m_EnemyAI;
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly T m_MeleeEnemyView;

        public KnockedDownState(Enemy<T> enemy, EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T meleeEnemyView)
        {
            m_Enemy = enemy;
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
            m_MeleeEnemyView = meleeEnemyView;
        }
        public void Enter()
        {
            m_EnemyAI.AgentEnabled = false;
            m_MeleeEnemyView.EnableRigidbody();
            
            m_MeleeEnemyView.DisabledAnimator();
        }
        
        public void Exit()
        {
            
        }
    }

    public interface IStateEnemy
    {
        void Enter();
        void Exit();
    }
}

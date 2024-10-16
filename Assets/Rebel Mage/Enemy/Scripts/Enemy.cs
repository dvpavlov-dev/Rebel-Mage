using System;
using System.Collections.Generic;
using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(DamageController))]
    public class Enemy<T> : MonoBehaviour where T : EnemyView
    {
        public T EnemyView;

        protected DamageController DmgController;
        protected EnemyAbilities<T> EnemyAbilities;
        protected EnemyAI<T> EnemyAI;
        public EnemyStateMachine<T> EnemySM;

        public int PointsForEnemy { get; protected set; }

        public virtual void InitEnemy(Configs.Configs configs, GameObject target, Action onDead)
        {
            EnemyView.Init(transform);
            EnemySM = new EnemyStateMachine<T>(this, EnemyAI, EnemyAbilities, EnemyView);

            DmgController = GetComponent<DamageController>();
            DmgController.OnDead = () => {
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
                [typeof(RagdollActivatedState<T>)] = new RagdollActivatedState<T>(enemy, enemyAI, enemyAbilities, meleeEnemyView),
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
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly EnemyAI<T> m_EnemyAI;
        private readonly T m_EnemyView;

        public MoveState(Enemy<T> enemy, EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T enemyView)
        {
            m_Enemy = enemy;
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
            m_EnemyView = enemyView;
        }

        public void Enter()
        {
            m_EnemyView.DisableRigidbody(() => 
            {
                m_EnemyAI.AgentEnabled = true;
            });
        }

        public void Exit() {}
    }

    class AttackState<T> : IStateEnemy where T : EnemyView
    {
        private readonly Enemy<T> m_Enemy;
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly EnemyAI<T> m_EnemyAI;
        private readonly T m_EnemyView;

        public AttackState(Enemy<T> enemy, EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T enemyView)
        {
            m_Enemy = enemy;
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
            m_EnemyView = enemyView;
        }

        public void Enter()
        {
            m_EnemyAI.AgentEnabled = false;

            // m_EnemyView.DisableRigidbody(() => {
            //     
            // });
        }

        public void Exit() {}
    }

    class RagdollActivatedState<T> : IStateEnemy where T : EnemyView
    {
        private readonly Enemy<T> m_Enemy;
        private readonly EnemyAbilities<T> m_EnemyAbilities;
        private readonly EnemyAI<T> m_EnemyAI;
        private readonly T m_EnemyView;

        public RagdollActivatedState(Enemy<T> enemy, EnemyAI<T> enemyAI, EnemyAbilities<T> enemyAbilities, T enemyView)
        {
            m_Enemy = enemy;
            m_EnemyAI = enemyAI;
            m_EnemyAbilities = enemyAbilities;
            m_EnemyView = enemyView;
        }
        public void Enter()
        {
            m_EnemyAI.AgentEnabled = false;

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

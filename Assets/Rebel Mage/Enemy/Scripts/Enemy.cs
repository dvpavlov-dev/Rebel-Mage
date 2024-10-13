using System;
using System.Collections.Generic;
using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    [RequireComponent(typeof(DamageController))]
    public class Enemy : MonoBehaviour
    {
        public MeleeEnemyView MeleeEnemyView;
        public Action OnDead;
        public EnemyStateMachine EnemySM;

        public int PointsForEnemy { get; protected set; }
        
        // public bool AnimatorEnabled 
        // {
        //     get => AnimationController != null && AnimationController.enabled;
        //     set 
        //     {
        //         if (AnimationController != null)
        //         {
        //             AnimationController.enabled = value;
        //         }
        //     }
        // }
        
        protected EnemyAbilities EnemyAbilities;
        protected EnemyAI EnemyAI;
        
        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;

            OnDead?.Invoke();
        }

        public virtual void InitEnemy(Configs.Configs configs, GameObject target)
        {
            MeleeEnemyView.Init(transform);
            EnemySM = new EnemyStateMachine(this, EnemyAI, EnemyAbilities, MeleeEnemyView);
        }
    }

    public class EnemyStateMachine
    {
        private readonly Dictionary<Type, IStateEnemy> m_States;
        
        private IStateEnemy m_ActiveState;
        
        public EnemyStateMachine(Enemy enemy, EnemyAI enemyAI, EnemyAbilities enemyAbilities, MeleeEnemyView meleeEnemyView)
        {
            m_States = new Dictionary<Type, IStateEnemy>
            {
                [typeof(MoveState)] = new MoveState(enemy, enemyAI, enemyAbilities, meleeEnemyView),
                [typeof(AttackState)] = new AttackState(enemy, enemyAI, enemyAbilities, meleeEnemyView),
                [typeof(KnockedDownState)] = new KnockedDownState(enemy, enemyAI, enemyAbilities, meleeEnemyView),
            };
            
            ChangeState<MoveState>();
        }

        public void ChangeState<TState>() where TState : class, IStateEnemy
        {
            m_ActiveState?.Exit();

            m_ActiveState = GetState<TState>();
            m_ActiveState.Enter();
        }
        
        private TState GetState<TState>() where TState : class, IStateEnemy => m_States[typeof(TState)] as TState;
    }

    class MoveState : IStateEnemy
    {
        private readonly Enemy m_Enemy;
        private readonly EnemyAI m_EnemyAI;
        private readonly EnemyAbilities m_EnemyAbilities;
        private readonly MeleeEnemyView m_MeleeEnemyView;

        public MoveState(Enemy enemy, EnemyAI enemyAI, EnemyAbilities enemyAbilities, MeleeEnemyView meleeEnemyView)
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
    
    class AttackState : IStateEnemy
    {
        private readonly Enemy m_Enemy;
        private readonly EnemyAI m_EnemyAI;
        private readonly EnemyAbilities m_EnemyAbilities;
        private readonly MeleeEnemyView m_MeleeEnemyView;

        public AttackState(Enemy enemy, EnemyAI enemyAI, EnemyAbilities enemyAbilities, MeleeEnemyView meleeEnemyView)
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
    
    class KnockedDownState : IStateEnemy
    {
        private readonly Enemy m_Enemy;
        private readonly EnemyAI m_EnemyAI;
        private readonly EnemyAbilities m_EnemyAbilities;
        private readonly MeleeEnemyView m_MeleeEnemyView;

        public KnockedDownState(Enemy enemy, EnemyAI enemyAI, EnemyAbilities enemyAbilities, MeleeEnemyView meleeEnemyView)
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

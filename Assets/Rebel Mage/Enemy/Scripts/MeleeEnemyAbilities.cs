﻿using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyAbilities : EnemyAbilities
    {
        private bool m_IsAttackStarted;
        private const string ANIMATION_NAME = "Mutant Punch";

        private void FixedUpdate()
        {
            if (!IsEnemyAbilitiesSetup) return;
            
            if (Vector3.Distance(transform.position, Target.transform.position) < 2)
            {
                if (!m_IsAttackStarted)
                {
                    m_IsAttackStarted = true;
                 
                    EnemyController.EnemySM.ChangeState<AttackState>();
                    EnemyController.AnimationController.SetTrigger(ANIMATION_NAME);
                    
                    foreach (AnimationClip clip in EnemyController.AnimationController.runtimeAnimatorController.animationClips)
                    {
                        if (clip.name == ANIMATION_NAME)
                        {
                            Invoke(nameof(OnEndAnimation), clip.length);
                        }
                    }
                }
            }
        }

        private void OnEndAnimation()
        {
            m_IsAttackStarted = false;
           
            EnemyController.EnemySM.ChangeState<MoveState>();
            
            foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, 2, Vector3.up))
            {
                if (hit.transform.CompareTag("Player") && hit.transform.GetComponent<IDamage>() is {} damageController)
                {
                    damageController.TakeDamage(Damage);
                }
            }
        }
    }
}
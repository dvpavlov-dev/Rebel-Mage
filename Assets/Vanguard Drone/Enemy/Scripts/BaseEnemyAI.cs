using UnityEngine;
using Vanguard_Drone.Enemy;

public class BaseEnemyAI : EnemyAI
{
    public override void SetupEnemyAI(float moveSpeed, GameObject target)
    {
        base.SetupEnemyAI(moveSpeed, target);
    }

    private void FixedUpdate()
    {
        if (!IsEnemySetup) return;
        
        Agent.SetDestination(Target.transform.position);
    }
}

namespace Rebel_Mage.Enemy
{
    public class MeleeEnemyAI : EnemyAI<MeleeEnemyView>
    {
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!IsEnemySetup) return;

            EnemyView.StartMoveAnimation(MoveCoefficient);
        }
    }
}

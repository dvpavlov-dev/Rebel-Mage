using UnityEngine;

namespace Rebel_Mage.Enemy
{
    public class BaseEnemyView : EnemyView
    {
        public ParticleSystem AttackEffect;

        public void ActivateAttackEffect()
        {
            AttackEffect.Play();
        }
    }
}

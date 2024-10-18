using System;

namespace Rebel_Mage.Spell_system
{
    public interface IDamage
    {
        public Action OnDead { get; set; }

        void InitHealthPoints(float maxHealth);
        void TakeDamage(float damage);
        void TakePeriodDamage(float damage, float interval, float time);
    }
}

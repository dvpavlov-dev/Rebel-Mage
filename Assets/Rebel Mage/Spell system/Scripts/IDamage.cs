namespace Rebel_Mage.Spell_system
{
    public interface IDamage
    {
        void TakeDamage(float damage);
        void TakePeriodDamage(float damage, float interval, float time);
    }
}

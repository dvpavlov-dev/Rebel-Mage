using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class IceBallController : SpellController<IceBallConfigSource>
    {
        public override void CastSpell()
        {
            Animator.SetTrigger(Config.AnimationName);
            Invoke(nameof(SpawnProjectile), 0.6f);
        }

        private void SpawnProjectile()
        {
            GameObject projectile = Instantiate(Config.SpellPrefab, SpellPoint.position, SpellPoint.rotation);
            projectile.GetComponent<Spell<IceBallConfigSource>>().Constructor(Owner, Config);
        }
    }
}

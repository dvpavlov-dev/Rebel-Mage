using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public class MagicSurgeController : SpellController<MagicSurgeConfigSource>
    {
        public override void CastSpell()
        {
            if (GetComponent<ICastSpells>() is {} actorCastSpell)
            {
                actorCastSpell.OnCastSpell(Config.AnimationTime);
            }
            
            Animator.SetLayerWeight(Animator.GetLayerIndex("Attack on the move"), 0);
            Animator.SetTrigger(Config.AnimationName);
            
            Invoke(nameof(SpawnProjectile), 0.8f);
            Invoke(nameof(OnEndAnimation), Config.AnimationTime);
        }
        
        private void SpawnProjectile()
        {
            GameObject spell = Instantiate(Config.SpellPrefab, SpellPoint.position, Quaternion.identity);
            spell.GetComponent<Spell<MagicSurgeConfigSource>>().Constructor(gameObject, Config);
        }

        private void OnEndAnimation()
        {
            Animator.SetLayerWeight(Animator.GetLayerIndex("Attack on the move"), 1);
        }
    }
}

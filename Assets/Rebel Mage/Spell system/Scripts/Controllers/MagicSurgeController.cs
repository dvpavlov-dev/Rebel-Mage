using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    [RequireComponent(typeof(AudioSource))]
    public class MagicSurgeController : SpellController<MagicSurgeConfigSource>
    {
        private GameObject chargeEffect;
        private AudioSource audioSource;
        
        public override void CastSpell()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.volume = 0.7f;
            }
            
            if (GetComponent<ICastSpells>() is {} actorCastSpell)
            {
                actorCastSpell.OnCastSpell(Config.AnimationTime);
            }
            
            Animator.SetLayerWeight(Animator.GetLayerIndex("Attack on the move"), 0);
            Animator.SetTrigger(Config.AnimationName);

            chargeEffect = Instantiate(Config.ChargeEffect, SpellPoint.position, Quaternion.identity);
            audioSource.clip = Config.ChargeSound;
            audioSource.Play();
            
            Invoke(nameof(SpawnProjectile), 0.8f);
            Invoke(nameof(OnEndAnimation), Config.AnimationTime);
        }
        
        private void SpawnProjectile()
        {
            Destroy(chargeEffect);
            audioSource.Stop();
            
            GameObject spell = Instantiate(Config.SpellPrefab, SpellPoint.position, Quaternion.identity);
            spell.GetComponent<Spell<MagicSurgeConfigSource>>().Constructor(gameObject, Config);
        }

        private void OnEndAnimation()
        {
            Animator.SetLayerWeight(Animator.GetLayerIndex("Attack on the move"), 1);
        }
    }
}

using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    [RequireComponent(typeof(AudioSource))]
    public class MagicSurgeController : SpellController<MagicSurgeConfigSource>
    {
        private GameObject _chargeEffect;
        private AudioSource _audioSource;
        
        public override void CastSpell()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
                _audioSource.playOnAwake = false;
                _audioSource.volume = 0.7f;
            }
            
            if (GetComponent<ICastSpells>() is {} actorCastSpell)
            {
                actorCastSpell.OnCastSpell(Config.AnimationTime);
            }
            
            Animator.SetLayerWeight(Animator.GetLayerIndex("Attack on the move"), 0);
            Animator.SetTrigger(Config.AnimationName);

            _chargeEffect = Instantiate(Config.ChargeEffect, SpellPoint.position, Quaternion.identity);
            _audioSource.clip = Config.ChargeSound;
            _audioSource.Play();
            
            Invoke(nameof(SpawnProjectile), 0.8f);
            Invoke(nameof(OnEndAnimation), Config.AnimationTime);
        }
        
        private void SpawnProjectile()
        {
            Destroy(_chargeEffect);
            _audioSource.Stop();
            
            GameObject spell = Instantiate(Config.SpellPrefab, SpellPoint.position, Quaternion.identity);
            spell.GetComponent<Spell<MagicSurgeConfigSource>>().Constructor(gameObject, Config);
        }

        private void OnEndAnimation()
        {
            Animator.SetLayerWeight(Animator.GetLayerIndex("Attack on the move"), 1);
        }
    }
}

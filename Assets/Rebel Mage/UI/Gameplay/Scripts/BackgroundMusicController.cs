using UnityEngine;
using UnityEngine.Serialization;

namespace Rebel_Mage.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusicController : MonoBehaviour
    {
        [FormerlySerializedAs("audioSource")]
        [SerializeField] private AudioSource _audioSource;
        [FormerlySerializedAs("chooseSpellClip")]
        [SerializeField] private AudioClip _chooseSpellClip;
        [FormerlySerializedAs("battleClip")]
        [SerializeField] private AudioClip _battleClip;

        public static BackgroundMusicController Instance;
    
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }
        
            Instance = this;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void ActivateChooseSpellClip()
        {
            _audioSource.clip = _chooseSpellClip;
            _audioSource.Play();
        }

        public void ActivateBattleClip()
        {
            _audioSource.clip = _battleClip;
            _audioSource.Play();
        }
    }
}
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip chooseSpellClip;
    [SerializeField] private AudioClip battleClip;

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
        audioSource = GetComponent<AudioSource>();
    }

    public void ActivateChooseSpellClip()
    {
        audioSource.clip = chooseSpellClip;
        audioSource.Play();
    }

    public void ActivateBattleClip()
    {
        audioSource.clip = battleClip;
        audioSource.Play();
    }
}

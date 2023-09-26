using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSound : MonoBehaviour
{
    public static SlimeSound instance;

    [SerializeField] private AudioSource jumpSource, creepSource, yellSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void PlayJumpSound(AudioClip clip)
    {
        
        jumpSource.PlayOneShot(clip);
        
    }

    public void PlayCreepSound(AudioClip clip)
    {
        
        creepSource.PlayOneShot(clip);
        
    }
    
    public void PlayYellSound(AudioClip clip)
    {
        yellSource.PlayOneShot(clip);
    }
    
}

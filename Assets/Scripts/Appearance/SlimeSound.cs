using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlimeSound : MonoBehaviour
{
    public static SlimeSound instance;
    [SerializeField] private AudioSource jumpSource, creepSource, yellSource, squeezeSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayJumpSound()
    {
        jumpSource.PlayOneShot(jumpSource.clip);
    }

    public void PlayCreepSound()
    {
        creepSource.pitch = Random.Range(0.75f, 0.9f);
        creepSource.PlayOneShot(creepSource.clip);
    }
    
    public void PlayYellSound()
    {
        yellSource.pitch = Random.Range(0.5f, 1.0f);
        if (Random.Range(0,1) < 0.3f)
        {
            yellSource.PlayOneShot(yellSource.clip);
        }
    }

    public void PlaySqueezeSound(float relDistance)
    {
        squeezeSource.volume = (float)(1.0 * 1 / relDistance);
        squeezeSource.Play();
    }
}

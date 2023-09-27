using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSound : MonoBehaviour
{
    [SerializeField] private AudioSource jumpSource, creepSource, yellSource;

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
        yellSource.PlayOneShot(creepSource.clip);
    }
}

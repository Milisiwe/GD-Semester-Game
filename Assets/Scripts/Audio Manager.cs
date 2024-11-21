using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource sfxSource;

    public AudioClip enemyWalk;
    public AudioClip enemyAttack;
    public AudioClip catWalk;
    
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager _instance; //单例模式

    private AudioSource audioSource;

    public AudioClip AwardBlueClip;
   
    private void Awake()
    {
        _instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    
    public void GetAwardBlueCollectible()
    {
        audioSource.PlayOneShot(AwardBlueClip);
    }
}

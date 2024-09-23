using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> clipList = new List<AudioClip>();
    public List<AudioClip> meleeList = new List<AudioClip>();
    public AudioSource audioSource;
    public AudioSource BgmAudio;
    public AudioClip SecondBgm;
    public static AudioManager instance = null;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    public void PlayOneShotAt(int id)
    {
        audioSource.PlayOneShot(clipList[id]);
    }


    public void PlayerSecondBGm()
    {
        BgmAudio.clip = SecondBgm;
        BgmAudio.Play();
    }
    public void playPlayerHitSound()
    {
        int ran = Random.Range(0,meleeList.Count);
        audioSource.PlayOneShot(meleeList[ran]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

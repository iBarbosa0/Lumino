using System;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
   
    public static SFXManager SfxManagerInstance;
    [SerializeField] private AudioClip backgroundmusicAudioClip;
    [SerializeField] private AudioClip letterfallingonwateClip;
    [SerializeField] private AudioClip gettingtherightletterclip;
    [SerializeField] private AudioClip bubblepopSfxAudioClip;
    [SerializeField] private AudioClip bubbleSplashSfxAudioClip;



    private void Awake()
    {
        if (SfxManagerInstance == null)
        {
            SfxManagerInstance = this;
        }
    }

    public void PlayBackgroundMusic()
    {
        AudioSource.PlayClipAtPoint(backgroundmusicAudioClip, transform.position);
    }
    public void PlayBubblePop(Vector3 bubbleposition)
    {
        AudioSource.PlayClipAtPoint(bubblepopSfxAudioClip, bubbleposition,10f);
    }
    public void PlayLetterFallingOnWater(Vector3 bubbleposition)
    {
        AudioSource.PlayClipAtPoint(bubbleSplashSfxAudioClip, bubbleposition);
    }
    public void PlayGettingRightLetter(Vector3 bubbleposition)
    {
        AudioSource.PlayClipAtPoint(gettingtherightletterclip, bubbleposition,1f);
    }
}

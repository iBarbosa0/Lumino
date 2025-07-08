using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip RiskSound;
    private void Awake()
    {
        instance = this;
    }

    public void PlayRiskSound()
    {
        AudioSource.PlayClipAtPoint(RiskSound, transform.position);
    }
}

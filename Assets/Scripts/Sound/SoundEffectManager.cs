using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static AudioClip dryShot, hitTarget, fireGun, reloadGun, startReload, activateTarget;
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dryShot = Resources.Load<AudioClip>("SoundEffects/DryShot");
        hitTarget = Resources.Load<AudioClip>("SoundEffects/Hit");
        fireGun = Resources.Load<AudioClip>("SoundEffects/Fire");
        reloadGun = Resources.Load<AudioClip>("SoundEffects/Reload");
        activateTarget = Resources.Load<AudioClip>("SoundEffects/Activated");
        startReload = Resources.Load<AudioClip>("SoundEffects/StartReload");
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "DryShot":
                audioSource.PlayOneShot(dryShot, 0.5f);
                break;
            case "Hit":
                audioSource.PlayOneShot(hitTarget, 0.5f);
                break;
            case "Fire":
                audioSource.PlayOneShot(fireGun, 0.1f);
                break;
            case "Reload":
                audioSource.PlayOneShot(reloadGun, 0.1f);
                break;
            case "StartReload":
                audioSource.PlayOneShot(startReload, 0.1f);
                break;
            case "ActivateTarget":
                audioSource.PlayOneShot(activateTarget, 0.1f);
                break;
        }
    }
}


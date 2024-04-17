using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField]
    private float minPitch = 0.8f;

    [SerializeField]
    private float maxPitch = 1.2f;

    public void PlayBowCharge()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Archery/String", transform.position);
    }

    public void PlayBowShoot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Archery/Whoosh", transform.position);
    }

    public void PlayDamage()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Character/Damage", transform.position);
    }

    public void PlayDeath()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Character/Die", transform.position);
    }

    public void PlayJump()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Character/Jump", transform.position);
    }

    public void PlayStomp()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Character/Land", transform.position);
    }

    public void PlaySwordWhoosh()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Sword/Whoosh", transform.position);
    }

    public void PlayRoll()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Character/Roll", transform.position);
    }
}